---
name: "wpf-settings-form"
description: "Pattern for building single-column settings forms in WPF with validation and read-only/editable fields"
domain: "desktop-ui"
confidence: "high"
source: "earned"
---

## Context

When building WPF applications that need configuration UI, a common requirement is a settings form that displays configuration values with some editable and some read-only. The form should validate edits, handle save/cancel/reset actions, and integrate with existing configuration services.

**ElBruno.OllamaMonitor Settings Window** demonstrates this pattern with 9 configuration keys (2 editable, 7 read-only).

## Patterns

### 1. Window Layout Structure

Use a Grid with 4 rows for consistent layout:

```xaml
<Grid Margin="20">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>      <!-- Header -->
        <RowDefinition Height="*"/>         <!-- Scrollable Form -->
        <RowDefinition Height="Auto"/>      <!-- Footer Warning -->
        <RowDefinition Height="Auto"/>      <!-- Action Buttons -->
    </Grid.RowDefinitions>
</Grid>
```

### 2. Reusable Styles in Window.Resources

Define styles for consistency across fields:

```xaml
<Window.Resources>
    <Style x:Key="SectionHeaderStyle" TargetType="TextBlock">
        <Setter Property="FontSize" Value="14"/>
        <Setter Property="FontWeight" Value="SemiBold"/>
        <Setter Property="Foreground" Value="#FF374151"/>
        <Setter Property="Margin" Value="0,16,0,8"/>
    </Style>
    <Style x:Key="FieldLabelStyle" TargetType="TextBlock">
        <Setter Property="FontSize" Value="12"/>
        <Setter Property="Foreground" Value="#FF6B7280"/>
        <Setter Property="Margin" Value="0,8,0,4"/>
    </Style>
    <Style x:Key="EditableFieldStyle" TargetType="TextBox">
        <Setter Property="Padding" Value="8"/>
        <Setter Property="FontSize" Value="12"/>
        <Setter Property="BorderBrush" Value="#FFD1D5DB"/>
        <Setter Property="BorderThickness" Value="1"/>
    </Style>
    <Style x:Key="ReadOnlyFieldStyle" TargetType="TextBox">
        <Setter Property="Padding" Value="8"/>
        <Setter Property="FontSize" Value="12"/>
        <Setter Property="BorderBrush" Value="#FFE5E7EB"/>
        <Setter Property="Background" Value="#FFF9FAFB"/>
        <Setter Property="IsReadOnly" Value="True"/>
    </Style>
</Window.Resources>
```

### 3. Form Fields with Inline Error Labels

Each editable field should have an error label below it:

```xaml
<TextBlock Style="{StaticResource FieldLabelStyle}" Text="Endpoint *"/>
<TextBox x:Name="EndpointTextBox" 
         Style="{StaticResource EditableFieldStyle}"
         ToolTip="The base URL of your Ollama API endpoint"/>
<TextBlock x:Name="EndpointErrorText" 
           Foreground="#FFDC2626" 
           FontSize="11" 
           Margin="0,2,0,0" 
           Visibility="Collapsed"/>
```

### 4. Footer Warning Banner

Use a Border for restart/warning notices:

```xaml
<Border Grid.Row="2" 
        Background="#FFFFFBEB" 
        BorderBrush="#FFFBBF24" 
        BorderThickness="1" 
        CornerRadius="4" 
        Padding="12" 
        Margin="0,12,0,12">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="⚠" FontSize="14" Foreground="#FFFBBF24" Margin="0,0,8,0"/>
        <TextBlock Text="Restart required for changes to take effect." 
                   FontSize="11" 
                   Foreground="#FF92400E"/>
    </StackPanel>
</Border>
```

### 5. Action Button Panel

Right-aligned buttons with primary styling:

```xaml
<StackPanel Grid.Row="3" Orientation="Horizontal" HorizontalAlignment="Right">
    <Button x:Name="ResetButton" Content="Reset to Defaults" Width="130" Height="32" 
            Margin="0,0,8,0" Click="OnResetClicked"/>
    <Button x:Name="CancelButton" Content="Cancel" Width="80" Height="32" 
            Margin="0,0,8,0" Click="OnCancelClicked"/>
    <Button x:Name="SaveButton" Content="Save" Width="80" Height="32" 
            Click="OnSaveClicked" IsDefault="True"
            Background="#FF3B82F6" Foreground="White"/>
</StackPanel>
```

### 6. Code-Behind: Load Settings on Window Loaded

Constructor takes service dependencies, async load on Loaded event:

```csharp
public SettingsWindow(AppSettingsService settingsService)
{
    _settingsService = settingsService;
    InitializeComponent();
    Loaded += async (_, _) => await LoadSettingsAsync();
}

private async Task LoadSettingsAsync()
{
    var settings = await _settingsService.LoadAsync(CancellationToken.None);
    
    EndpointTextBox.Text = settings.Endpoint;
    RefreshIntervalTextBox.Text = settings.RefreshIntervalSeconds.ToString();
    StartMinimizedCheckBox.IsChecked = settings.StartMinimizedToTray;
    // ... other fields
}
```

### 7. Save Handler: Last-Write-Wins Pattern

Always reload from disk before saving to prevent overwriting concurrent changes:

```csharp
private async void OnSaveClicked(object sender, RoutedEventArgs e)
{
    ClearErrors();

    // Validate editable fields
    var validation = ValidateEndpoint(EndpointTextBox.Text.Trim());
    if (!validation.ok)
    {
        ShowError(EndpointErrorText, validation.error!);
        return;
    }

    SaveButton.IsEnabled = false;
    SaveButton.Content = "Saving...";

    // CRITICAL: Reload from disk first (last-write-wins)
    var current = await _settingsService.LoadAsync(CancellationToken.None);
    
    // Apply user edits to reloaded object
    var updated = current with 
    { 
        Endpoint = EndpointTextBox.Text.Trim(),
        RefreshIntervalSeconds = int.Parse(RefreshIntervalTextBox.Text.Trim())
    };

    await _settingsService.SaveAsync(updated, CancellationToken.None);

    Hide();
}
```

### 8. Validation: Return Tuples for Clean Error Handling

Use tuples to avoid exception-based control flow:

```csharp
private static (bool ok, string? error) ValidateEndpoint(string endpoint)
{
    if (string.IsNullOrWhiteSpace(endpoint))
        return (false, "Endpoint cannot be empty.");
    
    if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
        return (false, "Endpoint must be a valid URI.");
    
    if (uri.Scheme != "http" && uri.Scheme != "https")
        return (false, "Endpoint must use http or https.");
    
    return (true, null);
}

private void ShowError(TextBlock errorLabel, string message)
{
    errorLabel.Text = message;
    errorLabel.Visibility = Visibility.Visible;
}

private void ClearErrors()
{
    EndpointErrorText.Visibility = Visibility.Collapsed;
    RefreshIntervalErrorText.Visibility = Visibility.Collapsed;
}
```

### 9. Reset Handler: Reload After Reset

Reset does NOT auto-close the window (user may want to review defaults):

```csharp
private async void OnResetClicked(object sender, RoutedEventArgs e)
{
    var result = System.Windows.MessageBox.Show(
        "Are you sure you want to reset all settings to defaults?",
        "Reset Settings",
        System.Windows.MessageBoxButton.YesNo,
        System.Windows.MessageBoxImage.Question);

    if (result != System.Windows.MessageBoxResult.Yes)
        return;

    ResetButton.IsEnabled = false;
    ResetButton.Content = "Resetting...";

    await _settingsService.ResetAsync(CancellationToken.None);
    await LoadSettingsAsync(); // Reload form fields from new defaults
}
```

### 10. Window Lifecycle: Hide-Instead-of-Close

Use PrepareForExit flag to allow reopening without re-instantiation:

```csharp
private bool _allowClose;

public void PrepareForExit() => _allowClose = true;

protected override void OnClosing(CancelEventArgs e)
{
    if (!_allowClose)
    {
        e.Cancel = true;
        Hide();
        return;
    }
    base.OnClosing(e);
}
```

## Examples

**File:** `src/ElBruno.OllamaMonitor/Windows/SettingsWindow.xaml`
- Lines 13-36: Reusable styles in Window.Resources
- Lines 53-68: Editable field with error label (Endpoint)
- Lines 128-141: Footer warning banner
- Lines 145-164: Action button panel

**File:** `src/ElBruno.OllamaMonitor/Windows/SettingsWindow.xaml.cs`
- Lines 9-16: Constructor with service dependency
- Lines 31-58: LoadSettingsAsync (populate fields from settings object)
- Lines 60-120: OnSaveClicked (last-write-wins pattern with validation)
- Lines 122-157: OnResetClicked (reset without auto-close)
- Lines 177-196: Validation methods (return tuples)

## Anti-Patterns

1. **Don't validate on field blur in Phase 1** — validation on Save is simpler and reduces UI churn (validate on blur is a Phase 2+ polish)
2. **Don't save without reloading first** — always reload from disk before saving to handle concurrent CLI changes
3. **Don't use exceptions for validation** — return tuples `(bool ok, string? error)` for clean control flow
4. **Don't auto-close on Reset** — user should review defaults before closing window
5. **Don't forget Visibility=Collapsed on error labels** — they should be hidden by default
6. **Don't use System.Windows.Forms.MessageBox in WPF** — use `System.Windows.MessageBox` (fully qualify to avoid ambiguity)
7. **Don't instantiate window on every show** — use hide-instead-of-close pattern to preserve state and avoid GC pressure

## Integration Notes

**Tray menu integration:**
- Insert "Settings…" menu item between related groups (after windows, before actions)
- Use ellipsis (…) to indicate modal dialog (Windows standard)
- Pass Action delegate to TrayIconService to avoid tight coupling

**App.xaml.cs lifecycle:**
- Instantiate once in LaunchTrayApplicationAsync (not on-demand)
- Add ShowSettingsWindow() public method: check IsVisible → Show() → Activate()
- Call PrepareForExit() and Close() in App.OnExit()

**Service layer integration:**
- Pass AppSettingsService (or similar) to constructor
- Use async/await throughout (never block UI thread)
- Inline validation with TODO markers for future service integration

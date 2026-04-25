# CI Optimization for Version Tags

**Author:** Tank (Platform Developer)  
**Date:** 2026-04-24  
**Requested by:** Bruno Capuano  
**Status:** Proposed

## Problem Statement

Current behavior: `publish.yml` workflow runs full CI on every GitHub Release publication (version tag).  
Cost impact: Full dotnet restore + build (Release config) + pack + publish on windows-latest per release.

**Current workflow cost estimate per release:**
- `actions/checkout@v4`: ~10 sec, ~1 min
- `actions/setup-dotnet@v4`: ~30 sec, ~1 min  
- `dotnet restore`: ~60 sec (no cache), ~2 min
- `dotnet build -c Release`: ~90 sec, ~3 min
- `Pack-Tool.ps1`: ~30 sec, ~1 min
- `NuGet login + push`: ~20 sec, ~1 min
- **Total:** ~5–6 min per release on windows-latest (~11–12 billing minutes)

Goal: Keep version tag CI functional but reduce GitHub Actions minute consumption.

---

## Research Findings

### Current publish.yml Analysis

**Trigger:** `release: types: [published]` + `workflow_dispatch`  
**Steps:**
1. Checkout code
2. Setup .NET 10.0.x
3. Determine version (from tag or input)
4. Restore NuGet packages (`dotnet restore`)
5. Build Release config (`dotnet build -c Release`)
6. Pack via PowerShell (`build\Pack-Tool.ps1`)
7. NuGet login (OIDC → temp API key)
8. Push to NuGet.org
9. Upload artifact

**No caching:** Restore runs fresh each time. Build cache not leveraged.

---

## Three Optimization Approaches

### Option A: Aggressive NuGet/Build Caching

**Strategy:** Cache NuGet packages and build outputs to skip restore + most of build.

**Implementation:**
```yaml
- uses: actions/setup-dotnet@v4
  with:
    dotnet-version: 10.0.x
    
- uses: actions/cache@v4
  with:
    path: ~/.nuget/packages
    key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj', '**/*.sln') }}
    restore-keys: |
      ${{ runner.os }}-nuget-

- uses: actions/cache@v4
  with:
    path: ./build/obj
    key: ${{ runner.os }}-build-${{ hashFiles('**/*.csproj') }}
    restore-keys: |
      ${{ runner.os }}-build-
```

**Expected Savings:**
- Restore: 0–60 sec (90% hit rate) → ~6 sec average
- Build: 30–50% faster with cached intermediates
- **Per-release savings:** ~1–2 min (33% reduction)

**Risks:**
- Cache key invalidation on dependency changes (mitigated by hashFiles)
- Windows runner cache stability (generally reliable)
- Cache size limits (~10 GB shared) — unlikely to hit

**Recommendation:** Low risk, high reward. Implement immediately.

---

### Option B: Selective Validation (Publish Only, Skip Full Build)

**Strategy:** On release publish, skip full build validation. Trust that code was tested on PR. Only run:
- Version validation
- Pack step
- Push to NuGet

**Implementation:**
```yaml
- name: Determine version
  # (run version validation script only)

- name: Pack (no build)
  # Use pre-built artifacts from PR validation or add --no-build flag

- name: Push to NuGet
```

**Expected Savings:** ~3 min (50% reduction — no restore, no build)

**Risks:**
- **HIGH:** Bypasses build validation. Version tag could publish broken packages.
- Violates principle of reproducible CI (tag CI should match PR CI).
- Recovery: Manual rebuild + republish required if broken package published.

**Not Recommended:** Too risky for a public NuGet package. Breaking release == reputation damage.

---

### Option C: Deferred Caching + Conditional Matrix Build

**Strategy:** Hybrid approach:
1. Implement Option A caching (immediate, low-risk win).
2. Add optional `verbose` workflow_dispatch input:
   - `verbose: false` (default) → uses cache, skip non-essential builds
   - `verbose: true` → full rebuild (for release candidates, validation)

**Implementation:**
```yaml
workflow_dispatch:
  inputs:
    version:
      description: Version to publish
      required: false
    verbose:
      description: Full rebuild (false = cached, true = clean)
      required: false
      default: 'false'

jobs:
  publish:
    steps:
      - name: Build (conditional)
        run: |
          if (${{ inputs.verbose }} -eq 'true') {
            dotnet build --no-incremental
          } else {
            dotnet build --incremental
          }
```

**Expected Savings:** ~1–2 min per default release (same as Option A)

**Risks:** Introduces manual decision point — risk of wrong choice during release.

**Recommendation:** Good future improvement, but Option A is the immediate win.

---

## Recommendation: **Implement Option A (Aggressive Caching)**

**Rationale:**
- ✅ Low risk (no behavioral changes)
- ✅ High reward (1–2 min saved per release)
- ✅ Cumulative benefit (cache grows across releases)
- ✅ Aligns with GitHub Actions best practices
- ✅ Reversible if issues arise

**Implementation Plan:**

1. Add NuGet cache step before `dotnet restore`
2. Add build output cache step before `dotnet build`
3. Test on next manual workflow_dispatch
4. Monitor cache hit rate via GitHub Actions logs
5. Iterate: Adjust cache paths if needed

**Estimated Effort:** 15 minutes (edit + 1 test release)

**Expected Result:** 
- Per-release savings: **1–2 minutes** (11–12 min → 9–10 min)
- Annual savings (12 releases): **12–24 GitHub Actions minutes**

---

## Appendix: Alternative Cost Reduction Strategies (Not Recommended)

### Strategy: Parallelize across runners
- **Cost:** No (would increase, not decrease)
- **Benefit:** Speed only
- **Status:** Not viable

### Strategy: Use self-hosted runners
- **Cost:** Infrastructure cost (Windows runner)
- **Benefit:** Effectively unlimited minutes
- **Status:** Out of scope for this task; consider for future if releases accelerate

### Strategy: Matrix builds by OS
- **Cost:** Higher (duplicates jobs)
- **Status:** Not viable

---

## Risk Assessment

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|-----------|
| Cache key mismatch | Low | Medium | hashFiles() dependency tracking |
| Stale build cache | Low | Low | Cache invalidation on .csproj change |
| NuGet cache corruption | Very Low | High | Disable cache if seen in logs |
| Cache size quota exceeded | Very Low | High | Monitor .nuget/packages size |

**Overall Risk Level:** 🟢 **Low** — Caching is a standard practice with GitHub Actions.

---

## Next Steps

1. Edit `.github/workflows/publish.yml` to add cache steps
2. Commit to `feature/sprint-improvements` branch
3. Test on next manual publish (or create test release)
4. Monitor cache hit rate for 2–3 releases
5. Document final savings in squad decisions

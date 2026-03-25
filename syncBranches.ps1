# SyncStudentBranches.ps1
$logFile = "GitSyncLog.txt"
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
"--- SYNC SESSION START: $timestamp ---" | Out-File $logFile -Append

Write-Host "--- STARTING GLOBAL CLASSROOM CLEANUP ---" -ForegroundColor Cyan

git fetch --all
$branches = git branch -r | Where-Object { $_ -notmatch "main" -and $_ -notmatch "->" }
$totalBranches = 0
$successCount = 0

foreach ($rawBranch in $branches) {
    $name = $rawBranch.Trim().Replace("origin/", "")
    $totalBranches++
    
    $header = "`n[$(Get-Date -Format 'HH:mm:ss')] PROCESSING: $name"
    Write-Host $header -ForegroundColor Yellow
    $header | Out-File $logFile -Append
    
    # Switch and Sync
    git checkout $name
    git reset --hard origin/$name
    
    # Remove junk files (DS_Store and _Recovery)
    "Cleaning .DS_Store and _Recovery files..." | Out-File $logFile -Append
    git rm -r --cached .DS_Store, **/.DS_Store, Assets/_Recovery, Assets/_Recovery.meta --ignore-unmatch | Out-File $logFile -Append
    
    # Execute Merge: Main -> Student Branch
    $mergeOutput = git merge main -Xours -m "Admin: Automatic sync with main and junk file cleanup"
    $mergeOutput | Out-File $logFile -Append
    Write-Output $mergeOutput
    
    # Push and record result
    if ($LASTEXITCODE -eq 0) {
        git push origin $name 2>&1 | Out-File $logFile -Append
        "SUCCESS: $name updated and pushed." | Out-File $logFile -Append
        $successCount++
    }
}

# Wrap up
git checkout main
$summary = "`n--- FINAL SUMMARY ---`nTotal: $totalBranches | Success: $successCount"
Write-Host $summary -ForegroundColor Green
$summary | Out-File $logFile -Append

# NEW: Automatically open the log for you to review
Start-Process notepad.exe $logFile

Write-Host "`nSync Complete. The log has been opened in Notepad." -ForegroundColor Cyan
Write-Host "Press any key to close this window..."
$Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") # This is a "hard" pause
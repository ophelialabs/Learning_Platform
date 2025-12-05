#!/usr/bin/env python3
"""
Script to create migrations using the .NET CLI
This works around version mismatches by using internal dotnet commands
"""
import subprocess
import os
import sys

os.chdir('/Users/meganmccaw/Downloads/Learning_Platform1-main-2/LP_app')

# Use the bundled dotnet ef
result = subprocess.run([
    'dotnet',
    'ef',
    'migrations',
    'add',
    'InitialCreate',
    '--output-dir',
    'Data/Migrations'
], capture_output=True, text=True)

print(result.stdout)
print(result.stderr, file=sys.stderr)
sys.exit(result.returncode)

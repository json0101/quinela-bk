<#
.SYNOPSIS
  Levanta todo el stack de Quinela (postgres + api + frontend) de una sola vez.

.DESCRIPTION
  Las migraciones del esquema 'sec' (userapp) y de la base 'quinela' las aplica
  quinela-api al ARRANCAR, solo si APPLY_MIGRATIONS=true. Son idempotentes (EF solo
  corre las pendientes), por eso este script las activa segun el caso:

    .\dev-up.ps1            -> arranque normal (NO migra: reinicio seguro)
    .\dev-up.ps1 -Migrate   -> arranca y aplica migraciones pendientes (1er deploy o tras agregar migraciones)
    .\dev-up.ps1 -Fresh     -> BORRA la base (volumen) y la recrea desde cero (implica -Migrate)
    .\dev-up.ps1 -Build     -> reconstruye las imagenes (combinable con lo anterior)

  Ejemplos:
    .\dev-up.ps1 -Fresh -Build      # primera vez / reset total
    .\dev-up.ps1 -Migrate -Build    # agregaste una migracion y cambiaste codigo
    .\dev-up.ps1                     # dia a dia
#>
[CmdletBinding()]
param(
  [switch]$Migrate,   # aplica migraciones pendientes en este arranque
  [switch]$Fresh,     # recrea la BD desde cero (down -v); implica -Migrate
  [switch]$Build      # reconstruye las imagenes (--build)
)

# NOTA: docker compose escribe su progreso en stderr. En PowerShell 5.1, con
# ErrorActionPreference=Stop eso aborta el script, asi que lo dejamos en Continue
# y validamos el resultado con $LASTEXITCODE.
$ErrorActionPreference = "Continue"
Set-Location $PSScriptRoot

# --- .env requerido por docker compose ---
if (-not (Test-Path ".\.env")) {
  Write-Error "Falta el archivo .env en $PSScriptRoot. Copia .env.example a .env y completa los valores."
  exit 1
}

# Aviso suave: la licencia de AutoMapper (la usa UserApp.Service) suele ir vacia en local.
$envText = Get-Content ".\.env" -Raw
if ($envText -match "USERAPP_AUTOMAPPER_LICENSE=\s*(\r?\n|$)") {
  Write-Host "Aviso: USERAPP_AUTOMAPPER_LICENSE esta vacio (ok en local; ponlo en produccion)." -ForegroundColor Yellow
}

# --- Reset total opcional ---
if ($Fresh) {
  Write-Host "==> Recreando la base desde cero (docker compose down -v)..." -ForegroundColor Cyan
  docker compose down -v
  if ($LASTEXITCODE -ne 0) { Write-Error "docker compose down -v fallo."; exit $LASTEXITCODE }
  $Migrate = $true   # una base nueva necesita migrar si o si
}

# --- Bandera de migraciones (la lee quinela-api al arrancar) ---
if ($Migrate) {
  $env:APPLY_MIGRATIONS = "true"
  Write-Host "==> APPLY_MIGRATIONS=true (se aplicaran migraciones pendientes)." -ForegroundColor Cyan
} else {
  $env:APPLY_MIGRATIONS = "false"
}

# --- Levantar el stack ---
$composeArgs = @("compose", "up", "-d")
if ($Build) { $composeArgs += "--build" }
Write-Host "==> docker $($composeArgs -join ' ')" -ForegroundColor Cyan
& docker @composeArgs
if ($LASTEXITCODE -ne 0) { Write-Error "docker compose up fallo."; exit $LASTEXITCODE }

# --- Estado final ---
Write-Host ""
docker compose ps --format "{{.Name}}: {{.Status}}"
Write-Host ""
Write-Host "API:      http://localhost:5060" -ForegroundColor Green
Write-Host "Frontend: http://localhost:3000" -ForegroundColor Green
Write-Host "Logs API: docker compose logs -f quinela-api" -ForegroundColor DarkGray

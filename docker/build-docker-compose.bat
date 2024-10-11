@ECHO OFF
SETLOCAL EnableExtensions DisableDelayedExpansion
CD "%~dp0"

@ECHO Deleting all BIN and OBJ folders ...
FOR /d /r . %%d IN (bin,obj) DO @IF EXIST "%%d" rd /s/q "%%d"
@ECHO BIN and OBJ folders successfully deleted

@ECHO ON
docker builder prune -af

:: Stop and remove containers, networks
docker compose ^
	-f docker-compose.yaml down

:: List & remove one or more containers
FOR /F %%f IN ('docker ps -a -qf "network=notino-network"') DO (
	docker rm %%f
)

:: Create and start container
docker compose -f docker-compose.yaml build --no-cache
docker compose -f docker-compose.yaml up -d --force-recreate

PAUSE



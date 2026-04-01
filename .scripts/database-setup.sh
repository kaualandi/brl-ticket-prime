#!/bin/bash

# ===== CONFIG =====
CONTAINER_NAME="ticketprime-sqlserver"
SQL_FILE_PATH=".db/ticketprime.sql"
CONTAINER_SQL_PATH="/tmp/script.sql"
DB_NAME="master"
SA_PASSWORD="TicketPrime@2026"

# ===== VALIDATIONS =====
if [ ! -f "$SQL_FILE_PATH" ]; then
  echo "❌ Arquivo SQL não encontrado: $SQL_FILE_PATH"
  exit 1
fi

echo "📦 Copiando script para o container..."
docker cp "$SQL_FILE_PATH" "$CONTAINER_NAME:$CONTAINER_SQL_PATH"

if [ $? -ne 0 ]; then
  echo "❌ Erro ao copiar o arquivo para o container"
  exit 1
fi

echo "🚀 Executando script no SQL Server..."

docker exec -i "$CONTAINER_NAME" \
/opt/mssql-tools18/bin/sqlcmd \
  -S localhost \
  -U sa \
  -P "$SA_PASSWORD" \
  -C \
  -d "$DB_NAME" \
  -i "$CONTAINER_SQL_PATH"

if [ $? -eq 0 ]; then
  echo "✅ Script executado com sucesso!"
else
  echo "❌ Erro ao executar o script"
  exit 1
fi
#!/bin/bash

# Define uma função para verificar se o SQL Server está pronto
wait_for_sqlserver() {
    echo "Aguardando o SQL Server iniciar..."
    for i in {1..50}; do
        /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Fiap@2024' -Q "SELECT 1" > /dev/null 2>&1
        if [ $? -eq 0 ]; then
            echo "SQL Server está pronto!"
            return 0
        fi
        echo "Ainda aguardando o SQL Server ($i)..."
        sleep 1
    done
    echo "SQL Server não iniciou dentro do tempo esperado."
    exit 1
}

#echo "Iniciando SQL Server"
#/opt/mssql/bin/sqlservr &

echo "Chamando funcao para verificar se o SQL Server subiu"
# Aguarda o SQL Server estar pronto
wait_for_sqlserver

# Executa o script SQL de inicialização
echo "Executando o script SQL de criacao de DATABASE..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Fiap@2024' -i /scripts/Criar_database.sql

echo "Executando o script de criacao de TABLES e SEED"
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Fiap@2024' -i /scripts/Setup_sql.sql

echo "Banco de dados criado e populado com sucesso!"

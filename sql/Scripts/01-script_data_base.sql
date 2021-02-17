CREATE DATABASE DemoExchange;

USE DemoExchange;

CREATE TABLE DemoExchange.TaxaCobranca(
	TaxaCobrancaId 		VARCHAR(50) 	NOT NULL,
	ValorTaxa 			NUMERIC(18,2) 	NOT NULL,
	TipoSegmento 		VARCHAR(15) 	NOT NULL,
	CriadoEm 			TIMESTAMP 		NOT NULL,
	AtualizadoEm 		TIMESTAMP 		NULL
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_general_ci;

ALTER TABLE DemoExchange.TaxaCobranca ADD CONSTRAINT TaxaCobranca_PK PRIMARY KEY (TaxaCobrancaId);

CREATE UNIQUE INDEX TaxaCobranca_TipoSegmento_IDX USING BTREE ON DemoExchange.TaxaCobranca (TipoSegmento);
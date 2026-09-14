-- ==============================================================================
-- 1. LIMPEZA DO AMBIENTE
-- Remoção das tabelas caso já existam.
-- A ordem DEVE ser inversa à criação para evitar conflito de Foreign Keys (FKs).
-- ==============================================================================
DROP TABLE IF EXISTS item_venda;
DROP TABLE IF EXISTS venda;
DROP TABLE IF EXISTS produto;

-- ==============================================================================
-- 2. TABELA DE PRODUTOS
-- Armazena o catálogo de itens disponíveis para venda.
-- ==============================================================================
CREATE TABLE produto (
    id_produto INTEGER GENERATED ALWAYS AS IDENTITY,
    nome VARCHAR(100) NOT NULL,
    preco NUMERIC(10,2) NOT NULL,
    quantidade_estoque INTEGER NOT NULL DEFAULT 0,

    -- [VALIDAÇÕES - PRODUTO]
    -- Define id_produto como chave primária (autoincremento e identificador único da tabela).
    CONSTRAINT pk_produto PRIMARY KEY (id_produto),

    -- Impede o cadastro de dois ou mais produtos com o exato mesmo nome.
    CONSTRAINT uq_produto_nome UNIQUE (nome),

    -- Garante que o nome do produto tenha pelo menos 2 caracteres válidos (remove espaços antes de contar).
    CONSTRAINT ck_produto_nome CHECK (LENGTH(TRIM(nome)) >= 2),

    -- Impede o cadastro ou atualização de produtos com preço zero ou negativo.
    CONSTRAINT ck_produto_preco CHECK (preco > 0),

    -- Impede que o saldo final de estoque fique negativo na tabela de produtos.
    CONSTRAINT ck_produto_estoque CHECK (quantidade_estoque >= 0)
);

-- ==============================================================================
-- 3. TABELA DE VENDAS
-- Armazena os cabeçalhos/transações de vendas efetuadas.
-- ==============================================================================
CREATE TABLE venda (
    id_venda INTEGER GENERATED ALWAYS AS IDENTITY,
    data_hora TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    valor_total NUMERIC(12,2) NOT NULL,
    ativa BOOLEAN NOT NULL DEFAULT TRUE,

    -- [VALIDAÇÕES - VENDA]
    -- Define id_venda como chave primária da transação.
    CONSTRAINT pk_venda PRIMARY KEY (id_venda),

    -- Exige que o valor total acumulado da venda seja estritamente superior a zero.
    CONSTRAINT ck_venda_valor_total CHECK (valor_total > 0)
);

-- ==============================================================================
-- 4. TABELA DE ITENS DA VENDA
-- Tabela de junção (N:N) que detalha os produtos pertencentes a cada venda.
-- ==============================================================================
CREATE TABLE item_venda (
    id_item_venda INTEGER GENERATED ALWAYS AS IDENTITY,
    id_venda INTEGER NOT NULL,
    id_produto INTEGER NOT NULL,
    quantidade INTEGER NOT NULL,
    valor_unitario NUMERIC(10,2) NOT NULL,

    -- [VALIDAÇÕES - ITEM_VENDA]
    -- Define id_item_venda como chave primária do registro do item.
    CONSTRAINT pk_item_venda PRIMARY KEY (id_item_venda),

    -- Chave estrangeira: garante que o item esteja associado a uma venda existente.
    CONSTRAINT fk_item_venda_venda FOREIGN KEY (id_venda) REFERENCES venda(id_venda),

    -- Chave estrangeira: garante que o produto associado exista na tabela de produtos.
    CONSTRAINT fk_item_venda_produto FOREIGN KEY (id_produto) REFERENCES produto(id_produto),

    -- Evita duplicidade: impede que o mesmo produto seja adicionado duas vezes na mesma venda.
    CONSTRAINT uq_item_venda_produto UNIQUE (id_venda, id_produto),

    -- Exige que a quantidade comprada de um item seja de no mínimo 1 unidade.
    CONSTRAINT ck_item_venda_quantidade CHECK (quantidade > 0),

    -- Garante que o valor unitário gravado no item no momento da venda seja superior a zero.
    CONSTRAINT ck_item_venda_valor_unitario CHECK (valor_unitario > 0)
);

-- ==============================================================================
-- 5. POPULANDO PRODUTOS
-- Inserção de 10 produtos iniciais com volumes altos de estoque.
-- ==============================================================================
INSERT INTO produto (nome, preco, quantidade_estoque)
VALUES
    ('Teclado Mecânico USB', 189.90, 500),
    ('Mouse Óptico Sem Fio', 79.90, 850),
    ('Monitor 27" 4K', 1499.90, 200),
    ('Headset Gamer 7.1', 299.00, 400),
    ('Webcam Full HD', 219.50, 350),
    ('Cadeira Ergonômica', 899.00, 150),
    ('Mousepad Extra Grande', 45.00, 1200),
    ('Suporte para Monitor', 119.90, 600),
    ('Hub USB-C 7 portas', 159.00, 450),
    ('Cabo HDMI 2.1 2m', 39.90, 1500);

-- ==============================================================================
-- 6. POPULANDO VENDAS (CABEÇALHO)
-- Registro inicial de 3 transações de venda.
-- ==============================================================================
INSERT INTO venda (valor_total)
VALUES 
    (269.80),   -- Venda ID 1
    (2258.80),  -- Venda ID 2
    (483.90);   -- Venda ID 3

-- ==============================================================================
-- 7. POPULANDO ITENS DAS VENDAS
-- Associação dos produtos vendidos às suas respetivas vendas e quantidades.
-- ==============================================================================
INSERT INTO item_venda (id_venda, id_produto, quantidade, valor_unitario)
VALUES 
    -- Itens vinculados à Venda 1 (Total calculado: (1 * 189.90) + (1 * 79.90) = 269.80)
    (1, 1, 1, 189.90), -- 1x Teclado Mecânico USB
    (1, 2, 1, 79.90),  -- 1x Mouse Óptico Sem Fio

    -- Itens vinculados à Venda 2 (Total calculado: (1 * 1499.90) + (2 * 299.00) + (1 * 119.90) = 2258.80)
    (2, 3, 1, 1499.90), -- 1x Monitor 27" 4K
    (2, 4, 2, 299.00),  -- 2x Headset Gamer 7.1
    (2, 8, 1, 119.90),  -- 1x Suporte para Monitor

    -- Itens vinculados à Venda 3 (Total calculado: (1 * 219.50) + (3 * 45.00) + (3 * 39.90) = 483.90)
    (3, 5, 1, 219.50), -- 1x Webcam Full HD
    (3, 7, 3, 45.00),  -- 3x Mousepad Extra Grande
    (3, 10, 3, 39.90); -- 3x Cabo HDMI 2.1 2m
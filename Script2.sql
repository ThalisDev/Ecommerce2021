create database bdecommerce
go

use bdecommerce
go

create table Categoria
(
    IdCategoria INT             IDENTITY    PRIMARY KEY,
    Nome        varchar(200)    not null,
)

create table Produto
(
    IdProduto   int             IDENTITY    PRIMARY key,
    Nome        varchar(max)    not null,
    Descricao   varchar(max)    not null,
    Valor       decimal(9,2)    not null,
    IdCategoria INT             references Categoria (IdCategoria), -- FK
)

insert into Categoria values('Informática')
insert into Categoria values('Brinquedos')

select * from Categoria

insert into Produto values ('Computador', 'Intel i9', 2000.00, 1)
insert into Produto values ('Computador', 'Intel i5', 2000.00, 1)
insert into Produto values ('Computador', 'Intel i3', 2000.00, 1)
insert into Produto values ('Hot Wheels', 'Carrinho', 10.00, 2)

select * from Produto
go

create table Cliente
(
    IdCliente int primary key identity,
    Nome varchar(100) not null,
    Email varchar(100) not null unique,
    Senha varchar(max) not null
)

insert into Cliente values ('Fulano', 'fulano@gmail.com', '123456')
insert into Cliente values ('Beltrano', 'beltrano@gmail.com', '123456')
go

create table Pedido
(
    IdPedido int primary key identity,
    IdCliente int not null,
    Data datetime not null,
    foreign key (IdCliente) references Cliente (IdCliente)
)
go

create table ItemPedido
(
    IdPedido int not null,
    IdProduto int not null,
    Quantidade int not null default 0,
    Preco decimal(9,2) not null,
    primary key (IdPedido, IdProduto),
    foreign key (IdPedido) references Pedido (IdPedido),
    foreign key (IdProduto) references Produto (IdProduto)
)
go

create view v_produto as
    select Produto.*, Categoria.Nome as Categoria
        from Produto
        join Categoria On Produto.IdCategoria = Categoria.IdCategoria
go

select * from v_produto
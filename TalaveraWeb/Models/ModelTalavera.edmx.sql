
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/09/2018 13:15:12
-- Generated from EDMX file: C:\Users\Alfa\Source\repos\Talavera\TalaveraWeb\Models\ModelTalavera.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Talavera];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BarroMaestra]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BarroMaestra];
GO
IF OBJECT_ID(N'[dbo].[EntregaPellas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntregaPellas];
GO
IF OBJECT_ID(N'[dbo].[MovimientosBarro]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovimientosBarro];
GO
IF OBJECT_ID(N'[dbo].[PreparacionBarro]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PreparacionBarro];
GO
IF OBJECT_ID(N'[dbo].[PreparacionPellas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PreparacionPellas];
GO
IF OBJECT_ID(N'[dbo].[prepBarro_prepPellas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[prepBarro_prepPellas];
GO
IF OBJECT_ID(N'[dbo].[Provedores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Provedores];
GO
IF OBJECT_ID(N'[dbo].[Recuperados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Recuperados];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'EntregaPellas'
CREATE TABLE [dbo].[EntregaPellas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaMovimiento] datetime  NULL,
    [Responsable] nvarchar(50)  NULL,
    [TipoMovimiento] nvarchar(1)  NULL,
    [CantidadPellas] int  NULL,
    [NumCarga] nvarchar(10)  NULL
);
GO

-- Creating table 'PreparacionBarro'
CREATE TABLE [dbo].[PreparacionBarro] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaPreparacion] datetime  NULL,
    [NumPreparado] nvarchar(10)  NULL,
    [BarroNegro] int  NULL,
    [BarroBlanco] int  NULL,
    [Recuperado] int  NULL,
    [EnPiedra] nvarchar(10)  NULL,
    [TiempoAgitacion] nvarchar(20)  NULL,
    [NumTambos] int  NULL,
    [DesperdicioMojado] nvarchar(10)  NULL,
    [Comentario] nvarchar(50)  NULL
);
GO

-- Creating table 'PreparacionPellas'
CREATE TABLE [dbo].[PreparacionPellas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Fuente] int  NULL,
    [NumCarga] nvarchar(10)  NULL,
    [FechaVaciado] datetime  NULL,
    [FechaLevantado] datetime  NULL,
    [FechaInicoPisado] datetime  NULL,
    [FechaFinPisado] datetime  NULL,
    [NumPeyas] int  NULL,
    [Restante] int  NULL,
    [CargaTotal] int  NULL
);
GO

-- Creating table 'prepBarro_prepPellas'
CREATE TABLE [dbo].[prepBarro_prepPellas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NumCarga] nvarchar(10)  NULL,
    [NumPreparado] nvarchar(10)  NULL
);
GO

-- Creating table 'Provedores'
CREATE TABLE [dbo].[Provedores] (
    [id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(250)  NULL,
    [Telefono] nvarchar(15)  NULL,
    [Telefono2] nvarchar(15)  NULL,
    [Numero] nvarchar(50)  NULL,
    [Calle] nvarchar(50)  NULL,
    [Colonia] nvarchar(50)  NULL,
    [Municipio] nvarchar(50)  NULL,
    [Estado] nvarchar(50)  NULL
);
GO

-- Creating table 'Recuperados'
CREATE TABLE [dbo].[Recuperados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaRecuperado] datetime  NULL,
    [Responsable] nvarchar(50)  NULL,
    [Cantidad] int  NULL,
    [NumRecuperado] nvarchar(10)  NULL,
    [TipoMovimiento] nvarchar(1)  NULL
);
GO

-- Creating table 'BarroMaestra'
CREATE TABLE [dbo].[BarroMaestra] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CodigoProducto] nvarchar(5)  NULL,
    [Capacidad] int  NULL,
    [Tipo] nvarchar(10)  NULL
);
GO

-- Creating table 'MovimientosBarro'
CREATE TABLE [dbo].[MovimientosBarro] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaMovimiento] datetime  NULL,
    [TipoMovimiento] nvarchar(2)  NULL,
    [CodigoProducto] nvarchar(5)  NULL,
    [Unidades] int  NULL,
    [Provedor] int  NULL,
    [Locacion] nvarchar(10)  NULL,
    [OrigenTranferencia] nvarchar(10)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'EntregaPellas'
ALTER TABLE [dbo].[EntregaPellas]
ADD CONSTRAINT [PK_EntregaPellas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PreparacionBarro'
ALTER TABLE [dbo].[PreparacionBarro]
ADD CONSTRAINT [PK_PreparacionBarro]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PreparacionPellas'
ALTER TABLE [dbo].[PreparacionPellas]
ADD CONSTRAINT [PK_PreparacionPellas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'prepBarro_prepPellas'
ALTER TABLE [dbo].[prepBarro_prepPellas]
ADD CONSTRAINT [PK_prepBarro_prepPellas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'Provedores'
ALTER TABLE [dbo].[Provedores]
ADD CONSTRAINT [PK_Provedores]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'Recuperados'
ALTER TABLE [dbo].[Recuperados]
ADD CONSTRAINT [PK_Recuperados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BarroMaestra'
ALTER TABLE [dbo].[BarroMaestra]
ADD CONSTRAINT [PK_BarroMaestra]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MovimientosBarro'
ALTER TABLE [dbo].[MovimientosBarro]
ADD CONSTRAINT [PK_MovimientosBarro]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
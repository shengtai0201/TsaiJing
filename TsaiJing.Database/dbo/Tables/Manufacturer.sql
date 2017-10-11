CREATE TABLE [dbo].[Manufacturer] (
    [ManufacturerId]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (64)  NOT NULL,
    [Address]            NVARCHAR (128) NOT NULL,
    [ContactPerson]      NVARCHAR (64)  NOT NULL,
    [ContactPersonPhone] VARCHAR (16)   NOT NULL,
    [Phone]              VARCHAR (16)   NOT NULL,
    [Fax]                VARCHAR (16)   NOT NULL,
    CONSTRAINT [PK_Manufacturer] PRIMARY KEY CLUSTERED ([ManufacturerId] ASC)
);


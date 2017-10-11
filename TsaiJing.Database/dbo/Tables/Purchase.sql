CREATE TABLE [dbo].[Purchase] (
    [PurchaseId]     INT            IDENTITY (1, 1) NOT NULL,
    [ManufacturerId] INT            NOT NULL,
    [UserId]         NVARCHAR (128) NOT NULL,
    [Date]           DATETIME       NOT NULL,
    [Remark]         NVARCHAR (256) NULL,
    CONSTRAINT [PK_ManufacturerProduct] PRIMARY KEY CLUSTERED ([PurchaseId] ASC),
    CONSTRAINT [FK_Purchase_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Purchase_Manufacturer] FOREIGN KEY ([ManufacturerId]) REFERENCES [dbo].[Manufacturer] ([ManufacturerId])
);


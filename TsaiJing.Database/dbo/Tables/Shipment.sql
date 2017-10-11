CREATE TABLE [dbo].[Shipment] (
    [ShipmentId]     INT            IDENTITY (1, 1) NOT NULL,
    [UserId]         NVARCHAR (128) NOT NULL,
    [UserRoleId]     NVARCHAR (128) NOT NULL,
    [CustomerId]     INT            NULL,
    [CustomerRoleId] NVARCHAR (128) NULL,
    [Date]           DATETIME       NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([ShipmentId] ASC),
    CONSTRAINT [FK_Order_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId]),
    CONSTRAINT [FK_Shipment_AspNetRoles] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]),
    CONSTRAINT [FK_Shipment_AspNetRoles1] FOREIGN KEY ([CustomerRoleId]) REFERENCES [dbo].[AspNetRoles] ([Id])
);


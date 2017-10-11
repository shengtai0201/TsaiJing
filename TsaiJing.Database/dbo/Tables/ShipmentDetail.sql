CREATE TABLE [dbo].[ShipmentDetail] (
    [ShipmentDetailId] INT IDENTITY (1, 1) NOT NULL,
    [ShipmentId]       INT NOT NULL,
    [ProductId]        INT NULL,
    [ProductDetailId]  INT NULL,
    [Quantity]         INT NOT NULL,
    [SubtotalAmount]   INT NOT NULL,
    CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED ([ShipmentDetailId] ASC),
    CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[Shipment] ([ShipmentId]),
    CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]),
    CONSTRAINT [FK_OrderDetail_ProductDetail] FOREIGN KEY ([ProductDetailId]) REFERENCES [dbo].[ProductDetail] ([ProductDetailId])
);


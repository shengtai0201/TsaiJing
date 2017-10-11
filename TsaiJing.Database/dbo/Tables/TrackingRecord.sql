CREATE TABLE [dbo].[TrackingRecord] (
    [TrackingRecordId] INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]       INT            NOT NULL,
    [ReferralTime]     DATETIME       NOT NULL,
    [BustUp]           INT            NULL,
    [BustDown]         INT            NULL,
    [MilkCapacity]     INT            NULL,
    [Abdominal]        INT            NULL,
    [Waist]            INT            NULL,
    [Hip]              INT            NULL,
    [LegLeft]          INT            NULL,
    [LegRight]         INT            NULL,
    [Design]           NVARCHAR (256) NULL,
    [Buy]              NVARCHAR (256) NULL,
    CONSTRAINT [PK_TrackingRecord] PRIMARY KEY CLUSTERED ([TrackingRecordId] ASC),
    CONSTRAINT [FK_TrackingRecord_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
);


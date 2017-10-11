CREATE TABLE [dbo].[Skin] (
    [CustomerId]           INT            NOT NULL,
    [ConditionDry]         BIT            NOT NULL,
    [ConditionOily]        BIT            NOT NULL,
    [ConditionSensitivity] BIT            NOT NULL,
    [ConditionMixed]       BIT            NOT NULL,
    [ImproveAcne]          BIT            NOT NULL,
    [ImproveSensitive]     BIT            NOT NULL,
    [ImproveWrinkle]       BIT            NOT NULL,
    [ImproveLargePores]    BIT            NOT NULL,
    [ImproveSpot]          BIT            NOT NULL,
    [ImproveDull]          BIT            NOT NULL,
    [ImprovePock]          BIT            NOT NULL,
    [ImproveOther]         NVARCHAR (64)  NULL,
    [Advice]               NVARCHAR (128) NULL,
    [Detail]               NVARCHAR (256) NULL,
    CONSTRAINT [PK_Skin] PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [FK_Skin_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
);


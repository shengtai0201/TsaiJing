CREATE TABLE [dbo].[Body] (
    [CustomerId]               INT            NOT NULL,
    [HealthSpine]              BIT            NOT NULL,
    [HealthBackPain]           BIT            NOT NULL,
    [HealthOther]              NVARCHAR (64)  NULL,
    [CurveChest]               BIT            NOT NULL,
    [CurveArm]                 BIT            NOT NULL,
    [CurveButtock]             BIT            NOT NULL,
    [CurveStomachWaistAbdomen] BIT            NOT NULL,
    [CurveThigh]               BIT            NOT NULL,
    [CurveCalf]                BIT            NOT NULL,
    [CurveFatSoft]             BIT            NOT NULL,
    [CurveFatHard]             BIT            NOT NULL,
    [CurveFatOrange]           BIT            NOT NULL,
    [CurveFatTangled]          BIT            NOT NULL,
    [CurveFatOther]            NVARCHAR (64)  NULL,
    [Diagnosis]                NVARCHAR (128) NULL,
    CONSTRAINT [PK_Body_1] PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [FK_Body_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
);


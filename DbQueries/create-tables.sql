USE [CurrencyWebApiService]
GO
/****** Object:  Table [dbo].[Currencies]    Script Date: 16/8/2021 09:25:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currencies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [char](3) NOT NULL,
	[TransactionLimitPerMonth] [decimal](18, 2) NOT NULL,
	[RestEnabled] [bit] NOT NULL,
	[RateApiEndpoint] [nvarchar](300) NOT NULL,
	[USDRateBase] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExchangeTransactions]    Script Date: 16/8/2021 09:25:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeTransactions](
	[TransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TransactionUtcDate] [datetime] NOT NULL,
	[OriginAmount] [decimal](18, 2) NOT NULL,
	[OriginCurrencyCode] [char](3) NOT NULL,
	[PurchasedAmount] [decimal](18, 5) NOT NULL,
	[DestinationCurrencyCode] [char](3) NOT NULL,
 CONSTRAINT [PK_ExchangeTransactions] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Currencies] ON 
GO
INSERT [dbo].[Currencies] ([Id], [Code], [TransactionLimitPerMonth], [RestEnabled], [RateApiEndpoint], [USDRateBase]) VALUES (1, N'USD', CAST(200.00 AS Decimal(18, 2)), 1, N'http://www.bancoprovincia.com.ar/Principal/Dolar', CAST(1.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Currencies] ([Id], [Code], [TransactionLimitPerMonth], [RestEnabled], [RateApiEndpoint], [USDRateBase]) VALUES (2, N'BRL', CAST(300.00 AS Decimal(18, 2)), 1, N'http://www.bancoprovincia.com.ar/Principal/Dolar', CAST(0.25 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Currencies] OFF
GO

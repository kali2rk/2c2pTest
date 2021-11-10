USE [TestDemo]
GO

/****** Object:  Table [dbo].[Transactions]    Script Date: 10/11/2021 9:02:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Transactions](
	[PkId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [nvarchar](50) NULL,
	[Amount] [float] NULL,
	[CurrencyCode] [nvarchar](3) NULL,
	[TransactionDate] [datetime] NULL,
	[Status] [nvarchar](10) NULL,
	[FormatType] [nvarchar](3) NULL,
	[TransactionDtCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[PkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DF_Transactions_TransactionDtCreated]  DEFAULT (getdate()) FOR [TransactionDtCreated]
GO



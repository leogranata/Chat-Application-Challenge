USE [ChatData]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [ChatData]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatRoom](
	[Name] [nvarchar](100) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ChatRoom] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Mobile] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NOT NULL,
	[DOB] [date] NOT NULL,
	[Username] [nvarchar](100) NULL,
	[ChatRoom] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChatRoom] ON 
GO
INSERT [dbo].[ChatRoom] ([Name], [Id]) VALUES (N'General', 1)
GO
INSERT [dbo].[ChatRoom] ([Name], [Id]) VALUES (N'Software', 2)
GO
INSERT [dbo].[ChatRoom] ([Name], [Id]) VALUES (N'Love', 3)
GO
SET IDENTITY_INSERT [dbo].[ChatRoom] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([UserId], [Email], [Mobile], [Password], [DOB], [Username], [ChatRoom]) VALUES (1, N'test@mail.com', N'123', N'pwd', CAST(N'1976-08-19' AS Date), N'Test', 3)
GO
INSERT [dbo].[User] ([UserId], [Email], [Mobile], [Password], [DOB], [Username], [ChatRoom]) VALUES (2, N'test2@mail.com', N'123', N'pwd', CAST(N'1976-08-19' AS Date), N'Test2', 3)
GO
INSERT [dbo].[User] ([UserId], [Email], [Mobile], [Password], [DOB], [Username], [ChatRoom]) VALUES (3, N'test3@mail.com', N'123', N'pwd', CAST(N'2000-01-01' AS Date), N'Test3', NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_ChatRoom] FOREIGN KEY([ChatRoom])
REFERENCES [dbo].[ChatRoom] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_ChatRoom]
GO
USE [master]
GO
ALTER DATABASE [ChatData] SET  READ_WRITE 
GO

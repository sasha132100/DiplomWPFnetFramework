USE [master]
GO
/****** Object:  Database [LocalMyDocsAppDB]    Script Date: 22.06.2023 3:38:07 ******/
CREATE DATABASE [LocalMyDocsAppDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LocalMyDocsAppDB', FILENAME = N'C:\Users\sasha\LocalMyDocsAppDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LocalMyDocsAppDB_log', FILENAME = N'C:\Users\sasha\LocalMyDocsAppDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [LocalMyDocsAppDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LocalMyDocsAppDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LocalMyDocsAppDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET  MULTI_USER 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LocalMyDocsAppDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LocalMyDocsAppDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [LocalMyDocsAppDB] SET QUERY_STORE = OFF
GO
USE [LocalMyDocsAppDB]
GO
/****** Object:  Table [dbo].[CreditCard]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditCard](
	[Id] [uniqueidentifier] NOT NULL,
	[Number] [nvarchar](30) NULL,
	[FIO] [nvarchar](255) NULL,
	[ExpiryDate] [nvarchar](15) NULL,
	[CVV] [int] NULL,
	[PhotoPage1] [image] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_CreditCard] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[INN]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[INN](
	[Id] [uniqueidentifier] NOT NULL,
	[Number] [nvarchar](25) NULL,
	[FIO] [nvarchar](255) NULL,
	[Gender] [char](1) NULL,
	[BirthDate] [datetime] NULL,
	[BirthPlace] [nvarchar](255) NULL,
	[RegistrationDate] [datetime] NULL,
	[PhotoPage1] [image] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_INN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Type] [nvarchar](150) NOT NULL,
	[Image] [image] NULL,
	[Priority] [int] NOT NULL,
	[IsHidden] [int] NOT NULL,
	[IsSelected] [int] NOT NULL,
	[DateCreation] [datetime] NOT NULL,
	[FolderId] [uniqueidentifier] NULL,
	[UserId] [int] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LastLogginedUser]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LastLogginedUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[Login] [nvarchar](255) NULL,
 CONSTRAINT [PK_LastLogginedUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Passport]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Passport](
	[Id] [uniqueidentifier] NOT NULL,
	[SerialNumber] [nvarchar](50) NULL,
	[FIO] [nvarchar](255) NULL,
	[Gender] [char](1) NULL,
	[BirthDate] [datetime] NULL,
	[BirthPlace] [nvarchar](255) NULL,
	[ResidencePlace] [nvarchar](255) NULL,
	[ByWhom] [nvarchar](255) NULL,
	[DivisionCode] [nvarchar](255) NULL,
	[GiveDate] [datetime] NULL,
	[FacePhoto] [image] NULL,
	[PhotoPage1] [image] NULL,
	[PhotoPage2] [image] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Passport] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Photo]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Photo](
	[Id] [uniqueidentifier] NOT NULL,
	[Image] [image] NOT NULL,
	[CollectionID] [uniqueidentifier] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polis]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polis](
	[Id] [uniqueidentifier] NOT NULL,
	[Number] [nvarchar](50) NULL,
	[FIO] [nvarchar](255) NULL,
	[Gender] [char](1) NULL,
	[BirthDate] [datetime] NULL,
	[PhotoPage1] [image] NULL,
	[PhotoPage2] [image] NULL,
	[ValidUntil] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Polis] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SNILS]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SNILS](
	[Id] [uniqueidentifier] NOT NULL,
	[Number] [nvarchar](50) NULL,
	[FIO] [nvarchar](255) NULL,
	[Gender] [char](1) NULL,
	[BirthDate] [datetime] NULL,
	[BirthPlace] [nvarchar](255) NULL,
	[RegistrationDate] [datetime] NULL,
	[PhotoPage1] [image] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_SNILS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Template]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Template](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Status] [nvarchar](10) NOT NULL,
	[Date] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateDocument]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateDocument](
	[Id] [uniqueidentifier] NOT NULL,
	[TemplateId] [uniqueidentifier] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_TemplateDocument] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateDocumentData]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateDocumentData](
	[Id] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](max) NULL,
	[TemplateObjectId] [uniqueidentifier] NOT NULL,
	[TemplateDocumentId] [uniqueidentifier] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_TemplateDocumentData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateObject]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateObject](
	[Id] [uniqueidentifier] NOT NULL,
	[Position] [int] NOT NULL,
	[Type] [nvarchar](150) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[TemplateId] [uniqueidentifier] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_TemplateObject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 22.06.2023 3:38:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Login] [nvarchar](255) NOT NULL,
	[Photo] [image] NULL,
	[AccessCode] [nvarchar](255) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_User_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Item] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Template] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[TemplateDocument] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[TemplateDocumentData] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[TemplateObject] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[CreditCard]  WITH CHECK ADD  CONSTRAINT [FK_CreditCard_Item] FOREIGN KEY([Id])
REFERENCES [dbo].[Item] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CreditCard] CHECK CONSTRAINT [FK_CreditCard_Item]
GO
ALTER TABLE [dbo].[INN]  WITH CHECK ADD  CONSTRAINT [FK_INN_Item] FOREIGN KEY([Id])
REFERENCES [dbo].[Item] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[INN] CHECK CONSTRAINT [FK_INN_Item]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_User]
GO
ALTER TABLE [dbo].[Passport]  WITH CHECK ADD  CONSTRAINT [FK_Passport_Item] FOREIGN KEY([Id])
REFERENCES [dbo].[Item] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Passport] CHECK CONSTRAINT [FK_Passport_Item]
GO
ALTER TABLE [dbo].[Photo]  WITH CHECK ADD  CONSTRAINT [FK_Photo_Item] FOREIGN KEY([CollectionID])
REFERENCES [dbo].[Item] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Photo] CHECK CONSTRAINT [FK_Photo_Item]
GO
ALTER TABLE [dbo].[Polis]  WITH CHECK ADD  CONSTRAINT [FK_Polis_Item] FOREIGN KEY([Id])
REFERENCES [dbo].[Item] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Polis] CHECK CONSTRAINT [FK_Polis_Item]
GO
ALTER TABLE [dbo].[SNILS]  WITH CHECK ADD  CONSTRAINT [FK_SNILS_Item] FOREIGN KEY([Id])
REFERENCES [dbo].[Item] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SNILS] CHECK CONSTRAINT [FK_SNILS_Item]
GO
ALTER TABLE [dbo].[Template]  WITH CHECK ADD  CONSTRAINT [FK_Template_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Template] CHECK CONSTRAINT [FK_Template_User]
GO
ALTER TABLE [dbo].[TemplateDocument]  WITH CHECK ADD  CONSTRAINT [FK_TemplateDocument_Item] FOREIGN KEY([Id])
REFERENCES [dbo].[Item] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TemplateDocument] CHECK CONSTRAINT [FK_TemplateDocument_Item]
GO
ALTER TABLE [dbo].[TemplateDocument]  WITH CHECK ADD  CONSTRAINT [FK_TemplateDocument_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Template] ([Id])
GO
ALTER TABLE [dbo].[TemplateDocument] CHECK CONSTRAINT [FK_TemplateDocument_Template]
GO
ALTER TABLE [dbo].[TemplateDocumentData]  WITH CHECK ADD  CONSTRAINT [FK_TemplateDocumentData_TemplateDocument] FOREIGN KEY([TemplateDocumentId])
REFERENCES [dbo].[TemplateDocument] ([Id])
GO
ALTER TABLE [dbo].[TemplateDocumentData] CHECK CONSTRAINT [FK_TemplateDocumentData_TemplateDocument]
GO
ALTER TABLE [dbo].[TemplateDocumentData]  WITH CHECK ADD  CONSTRAINT [FK_TemplateDocumentData_TemplateObject] FOREIGN KEY([TemplateObjectId])
REFERENCES [dbo].[TemplateObject] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TemplateDocumentData] CHECK CONSTRAINT [FK_TemplateDocumentData_TemplateObject]
GO
ALTER TABLE [dbo].[TemplateObject]  WITH CHECK ADD  CONSTRAINT [FK_TemplateObject_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Template] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TemplateObject] CHECK CONSTRAINT [FK_TemplateObject_Template]
GO
USE [master]
GO
ALTER DATABASE [LocalMyDocsAppDB] SET  READ_WRITE 
GO

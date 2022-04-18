USE [master]
GO
/****** Object:  Database [DynamicQuestionnaire]    Script Date: 2022/04/19 4:54:17 ******/
CREATE DATABASE [DynamicQuestionnaire]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DynamicQuestionnaire', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DynamicQuestionnaire.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DynamicQuestionnaire_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DynamicQuestionnaire_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DynamicQuestionnaire].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DynamicQuestionnaire] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET ARITHABORT OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DynamicQuestionnaire] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DynamicQuestionnaire] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DynamicQuestionnaire] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DynamicQuestionnaire] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET RECOVERY FULL 
GO
ALTER DATABASE [DynamicQuestionnaire] SET  MULTI_USER 
GO
ALTER DATABASE [DynamicQuestionnaire] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DynamicQuestionnaire] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DynamicQuestionnaire] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DynamicQuestionnaire] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DynamicQuestionnaire', N'ON'
GO
USE [DynamicQuestionnaire]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 2022/04/19 4:54:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [uniqueidentifier] NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questionnaires]    Script Date: 2022/04/19 4:54:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questionnaires](
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[Caption] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Questionnaires] PRIMARY KEY CLUSTERED 
(
	[QuestionnaireID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 2022/04/19 4:54:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[QuestionID] [uniqueidentifier] NOT NULL,
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[QuestionCategory] [varchar](50) NOT NULL,
	[QuestionTyping] [varchar](50) NOT NULL,
	[QuestionName] [nvarchar](50) NOT NULL,
	[QuestionRequired] [bit] NOT NULL,
	[QuestionAnswer] [nvarchar](500) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Problems] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Typings]    Script Date: 2022/04/19 4:54:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Typings](
	[TypingID] [uniqueidentifier] NOT NULL,
	[TypingName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Typings] PRIMARY KEY CLUSTERED 
(
	[TypingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAnswers]    Script Date: 2022/04/19 4:54:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAnswers](
	[UserID] [uniqueidentifier] NOT NULL,
	[QuestionID] [uniqueidentifier] NOT NULL,
	[QuestionTyping] [nvarchar](50) NOT NULL,
	[AnswerNum] [int] NOT NULL,
	[Answer] [nvarchar](500) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2022/04/19 4:54:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [uniqueidentifier] NOT NULL,
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](24) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Age] [int] NOT NULL,
	[AnswerDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (N'b25a79c7-1b96-4ec6-9887-4f215b61c675', N'常用問題')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (N'2a4bd007-7dac-404f-9cd0-60a5d46c63af', N'自訂問題')
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'a5bc4d84-1b1f-47c2-8bfc-10999e82b453', N't3', N't3', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:26:58.547' AS DateTime), CAST(N'2022-04-12T20:26:58.547' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'b32a91af-e4ca-48c9-a2e2-3301fa81d5a6', N't9', N't9', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:29:52.337' AS DateTime), CAST(N'2022-04-12T20:29:52.337' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'88a86fe6-c3de-46c4-950f-4f8239aaa6e7', N't13', N't13', 1, CAST(N'2022-04-15T00:00:00.000' AS DateTime), CAST(N'2022-04-15T00:00:00.000' AS DateTime), CAST(N'2022-04-15T00:52:54.847' AS DateTime), CAST(N'2022-04-15T00:52:54.847' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N't', N't', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:23:42.450' AS DateTime), CAST(N'2022-04-12T20:23:42.450' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'c0b5bb38-3eb6-43f5-a527-5756682e672b', N't6', N't6', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:28:40.633' AS DateTime), CAST(N'2022-04-12T20:28:40.633' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'e2604ecb-8690-4132-95df-5cd47580eb15', N't10', N't10', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T21:56:54.190' AS DateTime), CAST(N'2022-04-12T21:56:54.190' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'd108d54f-05dc-476d-91ad-939aad4bfdab', N't7', N't7', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:28:57.660' AS DateTime), CAST(N'2022-04-12T20:28:57.660' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'f5807276-78cf-4515-9c19-9ce7694a3840', N't12', N't12', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T22:03:19.210' AS DateTime), CAST(N'2022-04-12T22:03:19.210' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'test', N'test', 1, CAST(N'2022-04-13T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-13T17:09:53.097' AS DateTime), CAST(N'2022-04-13T17:09:53.097' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'513d0355-333a-4a55-a00c-c9327a26113f', N't5', N't5', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:28:02.777' AS DateTime), CAST(N'2022-04-12T20:28:02.777' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'660ce904-dfb4-40df-ba73-d5d1e8b30a7f', N't4', N't4', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:27:32.090' AS DateTime), CAST(N'2022-04-12T20:27:32.090' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'ef71f18e-4cd0-41f1-9cd4-dcc19f63faf6', N't11', N't11', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T21:59:41.447' AS DateTime), CAST(N'2022-04-12T21:59:41.447' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'e9af1926-5442-4290-b3b6-e2725a300e42', N't8', N't8', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:29:32.720' AS DateTime), CAST(N'2022-04-12T20:29:32.720' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'45a7ade8-5928-483e-a1ec-e399dca9d950', N't14', N't14', 0, CAST(N'2022-04-15T00:00:00.000' AS DateTime), CAST(N'2022-04-15T00:00:00.000' AS DateTime), CAST(N'2022-04-15T22:09:51.513' AS DateTime), CAST(N'2022-04-15T22:09:51.513' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'932bdea4-899a-490d-a585-fa8a47785981', N't2', N't2', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:26:31.593' AS DateTime), CAST(N'2022-04-12T20:26:31.593' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'837a291b-980b-4c9d-9512-067fa443a464', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'文字', N'test6', 0, N't6-answer1;t6-answer2;t6-answer3', CAST(N'2022-04-14T18:09:53.097' AS DateTime), CAST(N'2022-04-14T18:09:53.097' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'f4304d44-8174-4e03-9a32-0dfc6b6afde1', N'e2604ecb-8690-4132-95df-5cd47580eb15', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T21:56:54.190' AS DateTime), CAST(N'2022-04-12T21:56:54.190' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'32bb6d9c-e0a4-4f5b-9294-33febc1e726e', N'88a86fe6-c3de-46c4-950f-4f8239aaa6e7', N'自訂問題', N'文字', N't2', 1, N't2', CAST(N'2022-04-15T00:52:54.857' AS DateTime), CAST(N'2022-04-15T00:52:54.857' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'a0c9625d-a96b-4b38-a687-3410fe54a677', N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'常用問題', N'文字', N't3', 1, N't3', CAST(N'2022-04-12T20:24:18.273' AS DateTime), CAST(N'2022-04-12T20:24:18.273' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'8eebf622-4e71-4bc2-8067-35a7581d7f9a', N'd108d54f-05dc-476d-91ad-939aad4bfdab', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:28:57.660' AS DateTime), CAST(N'2022-04-12T20:28:57.660' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'c254af71-ca2e-41b5-bfcf-3bb3d93b7689', N'45a7ade8-5928-483e-a1ec-e399dca9d950', N'自訂問題', N'複選方塊', N't', 1, N't', CAST(N'2022-04-15T22:09:44.880' AS DateTime), CAST(N'2022-04-15T22:09:44.880' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'329cbb25-3aa9-4371-87ae-4c5e0fdf71cc', N'c0b5bb38-3eb6-43f5-a527-5756682e672b', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:28:40.633' AS DateTime), CAST(N'2022-04-12T20:28:40.633' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'cbd0e5c1-72a4-4bde-8955-53e24d790c25', N'88a86fe6-c3de-46c4-950f-4f8239aaa6e7', N'常用問題', N'複選方塊', N't', 1, N't', CAST(N'2022-04-15T00:52:42.650' AS DateTime), CAST(N'2022-04-15T00:52:42.650' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'25417891-33cd-4863-9c0d-56a31456b2e7', N'932bdea4-899a-490d-a585-fa8a47785981', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:26:31.593' AS DateTime), CAST(N'2022-04-12T20:26:31.593' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'68707b5e-a39d-4378-b62e-5b38105bd740', N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'自訂問題', N'單選方塊', N't2', 0, N't2', CAST(N'2022-04-12T20:24:08.963' AS DateTime), CAST(N'2022-04-12T20:24:08.963' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'ffad4c90-2ce2-4adb-b986-5c0dccc40b1b', N'f5807276-78cf-4515-9c19-9ce7694a3840', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T22:03:19.210' AS DateTime), CAST(N'2022-04-12T22:03:19.210' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'9e82e28f-f2b2-421c-8f77-688e0acc4882', N'660ce904-dfb4-40df-ba73-d5d1e8b30a7f', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:27:32.090' AS DateTime), CAST(N'2022-04-12T20:27:32.090' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'978757cc-d818-41a6-9132-72619fcc0855', N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'常用問題', N'複選方塊', N't4', 1, N't4', CAST(N'2022-04-12T20:24:40.963' AS DateTime), CAST(N'2022-04-12T20:24:40.963' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'986cec09-0d92-49cd-8f55-7b50aaab2198', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'單選方塊', N'test1', 0, N't1-answer1;t1-answer2;t1-answer3', CAST(N'2022-04-13T17:09:53.097' AS DateTime), CAST(N'2022-04-13T17:09:53.097' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'878b786b-7573-4ed3-a56d-8002f0ca1f33', N'a5bc4d84-1b1f-47c2-8bfc-10999e82b453', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:26:58.547' AS DateTime), CAST(N'2022-04-12T20:26:58.547' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'9e941802-e451-4ae2-b0f5-826de41c2194', N'45a7ade8-5928-483e-a1ec-e399dca9d950', N'自訂問題', N'文字', N't3', 1, N't3', CAST(N'2022-04-15T22:09:51.520' AS DateTime), CAST(N'2022-04-16T05:14:23.180' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'91742aeb-9d77-4448-95fd-94277e809d6a', N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:24:04.873' AS DateTime), CAST(N'2022-04-12T20:24:04.873' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'9eff871d-1aec-4fca-9571-a2ba32991fd5', N'b32a91af-e4ca-48c9-a2e2-3301fa81d5a6', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:29:52.337' AS DateTime), CAST(N'2022-04-12T20:29:52.337' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'文字', N'test3', 0, N't3-answer1;t3-answer2;t3-answer3', CAST(N'2022-04-14T18:09:53.097' AS DateTime), CAST(N'2022-04-14T18:09:53.097' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'複選方塊', N'test2', 0, N't2-answer1;t2-answer2;t2-answer3', CAST(N'2022-04-14T17:09:53.097' AS DateTime), CAST(N'2022-04-14T17:09:53.097' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'b2802fb8-3c7f-4d56-9fc3-bdf625c712d7', N'513d0355-333a-4a55-a00c-c9327a26113f', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:28:02.777' AS DateTime), CAST(N'2022-04-12T20:28:02.777' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'3c93c956-624d-47ff-b052-bebd39ed182b', N'ef71f18e-4cd0-41f1-9cd4-dcc19f63faf6', N'自訂問題', N'單選方塊', N't', 1, N't', CAST(N'2022-04-12T21:59:41.447' AS DateTime), CAST(N'2022-04-12T21:59:41.447' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'0e727b56-e9b5-434c-a295-cd527f110ab4', N'e9af1926-5442-4290-b3b6-e2725a300e42', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:29:32.720' AS DateTime), CAST(N'2022-04-12T20:29:32.720' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'c8a18cca-6d5f-4f18-88ad-d50f49ae49df', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'單選方塊', N'test4', 0, N't4-answer1;t4-answer2;t4-answer3', CAST(N'2022-04-13T17:09:53.097' AS DateTime), CAST(N'2022-04-13T17:09:53.097' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate]) VALUES (N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'複選方塊', N'test5', 0, N't5-answer1;t5-answer2;t5-answer3', CAST(N'2022-04-14T17:09:53.097' AS DateTime), CAST(N'2022-04-14T17:09:53.097' AS DateTime))
GO
INSERT [dbo].[Typings] ([TypingID], [TypingName]) VALUES (N'd7a18a03-515f-4981-b03e-6ba88a072397', N'複選方塊')
GO
INSERT [dbo].[Typings] ([TypingID], [TypingName]) VALUES (N'0fd9d755-1ca3-4b5f-a17a-9930c88cec06', N'文字')
GO
INSERT [dbo].[Typings] ([TypingID], [TypingName]) VALUES (N'b6f43a7b-962d-4d07-af59-bfea056beee5', N'單選方塊')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'複選方塊', 3, N'on')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'837a291b-980b-4c9d-9512-067fa443a464', N'文字', 1, N't61')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'837a291b-980b-4c9d-9512-067fa443a464', N'文字', 2, N't62')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'837a291b-980b-4c9d-9512-067fa443a464', N'文字', 3, N't63')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'986cec09-0d92-49cd-8f55-7b50aaab2198', N'單選方塊', 3, N'on')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 1, N't31')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 2, N't32')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 3, N't33')
GO
INSERT [dbo].[UserAnswers] ([UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'c8a18cca-6d5f-4f18-88ad-d50f49ae49df', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'e8ef7abe-8908-44e6-b93b-a4e3fc237995', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'tt', N'0123456789', N'tt@tt.com', 11, CAST(N'2022-04-19T04:37:31.283' AS DateTime))
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_CategoryID]  DEFAULT (newid()) FOR [CategoryID]
GO
ALTER TABLE [dbo].[Typings] ADD  CONSTRAINT [DF_Types_TypeID]  DEFAULT (newid()) FOR [TypingID]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Questionnaires_QuestionnaireID] FOREIGN KEY([QuestionnaireID])
REFERENCES [dbo].[Questionnaires] ([QuestionnaireID])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_Questionnaires_QuestionnaireID]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Questionnaires_QuestionnaireID] FOREIGN KEY([QuestionnaireID])
REFERENCES [dbo].[Questionnaires] ([QuestionnaireID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Questionnaires_QuestionnaireID]
GO
USE [master]
GO
ALTER DATABASE [DynamicQuestionnaire] SET  READ_WRITE 
GO

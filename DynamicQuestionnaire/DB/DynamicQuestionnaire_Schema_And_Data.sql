USE [master]
GO
/****** Object:  Database [DynamicQuestionnaire]    Script Date: 2022/05/12 11:47:43 ******/
CREATE DATABASE [DynamicQuestionnaire]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DynamicQuestionnaire', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DynamicQuestionnaire.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DynamicQuestionnaire_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DynamicQuestionnaire_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
/****** Object:  Table [dbo].[Categories]    Script Date: 2022/05/12 11:47:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [uniqueidentifier] NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
	[CommonQuestionID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommonQuestions]    Script Date: 2022/05/12 11:47:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommonQuestions](
	[CommonQuestionID] [uniqueidentifier] NOT NULL,
	[CommonQuestionName] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CommonQuestions] PRIMARY KEY CLUSTERED 
(
	[CommonQuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questionnaires]    Script Date: 2022/05/12 11:47:43 ******/
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
/****** Object:  Table [dbo].[Questions]    Script Date: 2022/05/12 11:47:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[QuestionID] [uniqueidentifier] NOT NULL,
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[QuestionCategory] [nvarchar](50) NOT NULL,
	[QuestionTyping] [nvarchar](50) NOT NULL,
	[QuestionName] [nvarchar](50) NOT NULL,
	[QuestionRequired] [bit] NOT NULL,
	[QuestionAnswer] [nvarchar](500) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[CommonQuestionID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Problems] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Typings]    Script Date: 2022/05/12 11:47:43 ******/
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
/****** Object:  Table [dbo].[UserAnswers]    Script Date: 2022/05/12 11:47:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAnswers](
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[QuestionID] [uniqueidentifier] NOT NULL,
	[QuestionTyping] [nvarchar](50) NOT NULL,
	[AnswerNum] [int] NOT NULL,
	[Answer] [nvarchar](500) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2022/05/12 11:47:43 ******/
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
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CommonQuestionID]) VALUES (N'b25a79c7-1b96-4ec6-9887-4f215b61c675', N'常用問題', NULL)
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CommonQuestionID]) VALUES (N'cfa3db08-04a2-476e-b11e-534528817489', N'CommonQuestion1', N'd7c81e24-6435-4275-9d76-918867daedab')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CommonQuestionID]) VALUES (N'2a4bd007-7dac-404f-9cd0-60a5d46c63af', N'自訂問題', NULL)
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CommonQuestionID]) VALUES (N'7fa31779-6f74-4788-88d5-b778470a683b', N'CommonQuestion2', N'2a59fd0f-926d-47f8-9bfe-2e1dd9eabc05')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CommonQuestionID]) VALUES (N'960ba609-1067-4ebb-835d-d73ddca29cf8', N'CommonQuestion5', N'1cab573c-3daf-4549-960d-50bb03a1f611')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [CommonQuestionID]) VALUES (N'ddb48de0-0e3e-4663-8a87-ec391be365c0', N'CommonQuestion3', N'c54db6cd-4cc8-4930-b406-5ff17a14826c')
GO
INSERT [dbo].[CommonQuestions] ([CommonQuestionID], [CommonQuestionName], [CreateDate], [UpdateDate]) VALUES (N'2a59fd0f-926d-47f8-9bfe-2e1dd9eabc05', N'CommonQuestion2', CAST(N'2022-04-25T04:34:54.583' AS DateTime), CAST(N'2022-04-25T05:26:01.220' AS DateTime))
GO
INSERT [dbo].[CommonQuestions] ([CommonQuestionID], [CommonQuestionName], [CreateDate], [UpdateDate]) VALUES (N'1cab573c-3daf-4549-960d-50bb03a1f611', N'CommonQuestion5', CAST(N'2022-05-08T16:00:01.817' AS DateTime), CAST(N'2022-05-08T16:01:02.350' AS DateTime))
GO
INSERT [dbo].[CommonQuestions] ([CommonQuestionID], [CommonQuestionName], [CreateDate], [UpdateDate]) VALUES (N'c54db6cd-4cc8-4930-b406-5ff17a14826c', N'CommonQuestion3', CAST(N'2022-04-25T04:35:15.643' AS DateTime), CAST(N'2022-04-25T06:40:08.750' AS DateTime))
GO
INSERT [dbo].[CommonQuestions] ([CommonQuestionID], [CommonQuestionName], [CreateDate], [UpdateDate]) VALUES (N'd7c81e24-6435-4275-9d76-918867daedab', N'CommonQuestion1', CAST(N'2022-04-25T04:34:41.387' AS DateTime), CAST(N'2022-04-25T04:34:41.387' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'a5bc4d84-1b1f-47c2-8bfc-10999e82b453', N'questionnaire3', N'questionnaire3Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:26:58.547' AS DateTime), CAST(N'2022-04-12T20:26:58.547' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'b32a91af-e4ca-48c9-a2e2-3301fa81d5a6', N'questionnaire9', N'questionnaire9Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:29:52.337' AS DateTime), CAST(N'2022-04-12T20:29:52.337' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'88a86fe6-c3de-46c4-950f-4f8239aaa6e7', N'questionnaire13', N'questionnaire13Des', 0, CAST(N'2022-04-15T00:00:00.000' AS DateTime), CAST(N'2022-04-15T00:00:00.000' AS DateTime), CAST(N'2022-04-15T00:52:54.847' AS DateTime), CAST(N'2022-04-15T00:52:54.847' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'questionnaire1', N'questionnaire1Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:23:42.450' AS DateTime), CAST(N'2022-04-12T20:23:42.450' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'c0b5bb38-3eb6-43f5-a527-5756682e672b', N'questionnaire6', N'questionnaire6Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:28:40.633' AS DateTime), CAST(N'2022-04-12T20:28:40.633' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'baf455b3-6663-4031-aec4-5a4635e34c36', N'questionnaire16', N'questionnaire16Des', 0, CAST(N'2022-05-05T00:00:00.000' AS DateTime), CAST(N'2022-05-11T00:00:00.000' AS DateTime), CAST(N'2022-05-05T09:58:03.547' AS DateTime), CAST(N'2022-05-11T12:42:22.397' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'e2604ecb-8690-4132-95df-5cd47580eb15', N'questionnaire10', N'questionnaire10Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T21:56:54.190' AS DateTime), CAST(N'2022-04-12T21:56:54.190' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'questionnaire15', N'questionnaire15Des', 1, CAST(N'2022-04-20T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-20T16:45:17.983' AS DateTime), CAST(N'2022-04-20T16:45:17.983' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'd108d54f-05dc-476d-91ad-939aad4bfdab', N'questionnaire7', N'questionnaire7Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:28:57.660' AS DateTime), CAST(N'2022-04-12T20:28:57.660' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'f5807276-78cf-4515-9c19-9ce7694a3840', N'questionnaire12', N'questionnaire12Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T22:03:19.210' AS DateTime), CAST(N'2022-04-12T22:03:19.210' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'questionnaireTest', N'questionnaireTestDes', 1, CAST(N'2022-04-13T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-13T17:09:53.097' AS DateTime), CAST(N'2022-04-13T17:09:53.097' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'513d0355-333a-4a55-a00c-c9327a26113f', N'questionnaire5', N'questionnaire5Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:28:02.777' AS DateTime), CAST(N'2022-04-12T20:28:02.777' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'660ce904-dfb4-40df-ba73-d5d1e8b30a7f', N'questionnaire4', N'questionnaire4Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:27:32.090' AS DateTime), CAST(N'2022-04-12T20:27:32.090' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'ef71f18e-4cd0-41f1-9cd4-dcc19f63faf6', N'questionnaire11', N'questionnaire11Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T21:59:41.447' AS DateTime), CAST(N'2022-04-12T21:59:41.447' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'e9af1926-5442-4290-b3b6-e2725a300e42', N'questionnaire8', N'questionnaire8Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:29:32.720' AS DateTime), CAST(N'2022-04-12T20:29:32.720' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'45a7ade8-5928-483e-a1ec-e399dca9d950', N'questionnaire14', N'questionnaire14Des', 0, CAST(N'2022-04-15T00:00:00.000' AS DateTime), CAST(N'2022-04-15T00:00:00.000' AS DateTime), CAST(N'2022-04-15T22:09:51.513' AS DateTime), CAST(N'2022-04-15T22:09:51.513' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'questionnaire17', N'questionnaire17Des', 0, CAST(N'2022-05-08T00:00:00.000' AS DateTime), CAST(N'2022-05-08T00:00:00.000' AS DateTime), CAST(N'2022-05-08T03:30:14.927' AS DateTime), CAST(N'2022-05-09T12:44:02.123' AS DateTime))
GO
INSERT [dbo].[Questionnaires] ([QuestionnaireID], [Caption], [Description], [IsEnable], [StartDate], [EndDate], [CreateDate], [UpdateDate]) VALUES (N'932bdea4-899a-490d-a585-fa8a47785981', N'questionnaire2', N'questionnaire2Des', 1, CAST(N'2022-04-12T00:00:00.000' AS DateTime), NULL, CAST(N'2022-04-12T20:26:31.593' AS DateTime), CAST(N'2022-04-12T20:26:31.593' AS DateTime))
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'a31ef11e-161c-4be3-9e43-01cef7cbdee1', N'baf455b3-6663-4031-aec4-5a4635e34c36', N'常用問題', N'複選方塊', N'cQuestion1', 1, N'cQuestion1-answer1;cQuestion1-answer2;cQuestion1-answer3', CAST(N'2022-05-06T15:58:29.340' AS DateTime), CAST(N'2022-05-06T15:58:29.340' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'837a291b-980b-4c9d-9512-067fa443a464', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'文字', N'question6', 0, N'question6-answer1;question6-answer2;question6-answer3', CAST(N'2022-04-14T18:09:53.097' AS DateTime), CAST(N'2022-04-14T18:09:53.097' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'0fb5cba4-651c-4900-9ab7-0a6c04273424', N'61e972a5-ec4f-45e7-9161-c3d2fc65a0da', N'常用問題', N'複選方塊', N'cQuestion3', 0, N'cQuestion3-answer1;cQuestion3-answer2;cQuestion3-answer3', CAST(N'2022-04-25T04:37:54.410' AS DateTime), CAST(N'2022-04-25T04:37:54.410' AS DateTime), N'2a59fd0f-926d-47f8-9bfe-2e1dd9eabc05')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'f4304d44-8174-4e03-9a32-0dfc6b6afde1', N'e2604ecb-8690-4132-95df-5cd47580eb15', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T21:56:54.190' AS DateTime), CAST(N'2022-04-12T21:56:54.190' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'32bb6d9c-e0a4-4f5b-9294-33febc1e726e', N'88a86fe6-c3de-46c4-950f-4f8239aaa6e7', N'自訂問題', N'文字', N't2', 1, N't2', CAST(N'2022-04-15T00:52:54.857' AS DateTime), CAST(N'2022-04-15T00:52:54.857' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'a0c9625d-a96b-4b38-a687-3410fe54a677', N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'自訂問題', N'文字', N't3', 1, N't3', CAST(N'2022-04-12T20:24:18.273' AS DateTime), CAST(N'2022-04-12T20:24:18.273' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'8eebf622-4e71-4bc2-8067-35a7581d7f9a', N'd108d54f-05dc-476d-91ad-939aad4bfdab', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:28:57.660' AS DateTime), CAST(N'2022-04-12T20:28:57.660' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'c254af71-ca2e-41b5-bfcf-3bb3d93b7689', N'45a7ade8-5928-483e-a1ec-e399dca9d950', N'自訂問題', N'複選方塊', N't', 1, N't', CAST(N'2022-04-15T22:09:44.880' AS DateTime), CAST(N'2022-04-15T22:09:44.880' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'329cbb25-3aa9-4371-87ae-4c5e0fdf71cc', N'c0b5bb38-3eb6-43f5-a527-5756682e672b', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:28:40.633' AS DateTime), CAST(N'2022-04-12T20:28:40.633' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'cbd0e5c1-72a4-4bde-8955-53e24d790c25', N'88a86fe6-c3de-46c4-950f-4f8239aaa6e7', N'自訂問題', N'複選方塊', N't', 1, N't', CAST(N'2022-04-15T00:52:42.650' AS DateTime), CAST(N'2022-04-15T00:52:42.650' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'25417891-33cd-4863-9c0d-56a31456b2e7', N'932bdea4-899a-490d-a585-fa8a47785981', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:26:31.593' AS DateTime), CAST(N'2022-04-12T20:26:31.593' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'68707b5e-a39d-4378-b62e-5b38105bd740', N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'自訂問題', N'單選方塊', N't2', 0, N't2', CAST(N'2022-04-12T20:24:08.963' AS DateTime), CAST(N'2022-04-12T20:24:08.963' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'ffad4c90-2ce2-4adb-b986-5c0dccc40b1b', N'f5807276-78cf-4515-9c19-9ce7694a3840', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T22:03:19.210' AS DateTime), CAST(N'2022-04-12T22:03:19.210' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'自訂問題', N'文字', N'question2', 1, N'question2-answer1;question2-answer2;question2-answer3', CAST(N'2022-04-20T16:41:43.730' AS DateTime), CAST(N'2022-04-20T16:41:43.730' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'4c3e5a1f-752e-4987-86ef-649bee12240b', N'61e972a5-ec4f-45e7-9161-c3d2fc65a0da', N'常用問題', N'單選方塊', N'cQuestion5', 0, N'cQuestion5-answer1;cQuestion5-answer2;cQuestion5-answer3', CAST(N'2022-04-25T04:56:46.930' AS DateTime), CAST(N'2022-04-25T04:56:46.930' AS DateTime), N'2a59fd0f-926d-47f8-9bfe-2e1dd9eabc05')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'c1615277-65d6-4cd4-941d-663152ea348f', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'自訂問題', N'單選方塊', N'question3', 1, N'question3-answer1;question3-answer2;question3-answer3', CAST(N'2022-04-20T16:42:19.027' AS DateTime), CAST(N'2022-04-20T16:42:19.027' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'9e82e28f-f2b2-421c-8f77-688e0acc4882', N'660ce904-dfb4-40df-ba73-d5d1e8b30a7f', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:27:32.090' AS DateTime), CAST(N'2022-04-12T20:27:32.090' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'978757cc-d818-41a6-9132-72619fcc0855', N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'自訂問題', N'複選方塊', N't4', 1, N't4', CAST(N'2022-04-12T20:24:40.963' AS DateTime), CAST(N'2022-04-12T20:24:40.963' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'5f5d884e-b7bd-445a-b87d-735fb23c95c6', N'f8297932-0149-4f0f-926d-e89492b92cb0', N'常用問題', N'複選方塊', N'cQuestion3', 0, N'cQuestion3-answer1;cQuestion3-answer2;cQuestion3-answer3', CAST(N'2022-04-25T06:40:07.140' AS DateTime), CAST(N'2022-04-25T06:40:07.140' AS DateTime), N'c54db6cd-4cc8-4930-b406-5ff17a14826c')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'3182864f-2bb0-4070-8b98-780b50a50c42', N'b972e9e5-8d96-4d89-ab50-c6a534e11ce3', N'常用問題', N'複選方塊', N'cQuestion1', 1, N'cQuestion1-answer1;cQuestion1-answer2;cQuestion1-answer3', CAST(N'2022-04-25T04:34:41.400' AS DateTime), CAST(N'2022-04-25T04:34:41.400' AS DateTime), N'd7c81e24-6435-4275-9d76-918867daedab')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'986cec09-0d92-49cd-8f55-7b50aaab2198', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'單選方塊', N'question1', 1, N'question1-answer1;question1-answer2;question1-answer3', CAST(N'2022-04-13T17:09:53.097' AS DateTime), CAST(N'2022-04-13T17:09:53.097' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'a8703c65-e2f7-4339-bf9f-7fb556978752', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'自訂問題', N'單選方塊', N'question6', 0, N'question6-answer1;questio6-answer2;question6-answer3', CAST(N'2022-04-20T16:44:14.263' AS DateTime), CAST(N'2022-04-20T16:44:14.263' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'878b786b-7573-4ed3-a56d-8002f0ca1f33', N'a5bc4d84-1b1f-47c2-8bfc-10999e82b453', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:26:58.547' AS DateTime), CAST(N'2022-04-12T20:26:58.547' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'45e57fc1-f429-4083-9183-80937884374e', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'自訂問題', N'複選方塊', N'question1', 1, N'question1-answer1;question1-answer2;question1-answer3', CAST(N'2022-04-20T16:40:40.987' AS DateTime), CAST(N'2022-04-20T16:40:40.987' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'9e941802-e451-4ae2-b0f5-826de41c2194', N'45a7ade8-5928-483e-a1ec-e399dca9d950', N'自訂問題', N'文字', N't3', 1, N't3', CAST(N'2022-04-15T22:09:51.520' AS DateTime), CAST(N'2022-04-16T05:14:23.180' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'ee0635e5-ad51-472e-9277-8c31105007d5', N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'自訂問題', N'複選方塊', N'question3', 1, N'question3-answer1;question3-answer2;question3-answer3', CAST(N'2022-05-08T03:32:27.190' AS DateTime), CAST(N'2022-05-08T03:32:27.190' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'2ee68502-5017-43bb-8690-908f9b621fc7', N'd9a7c5d5-41c2-4d0b-8266-56887ea101e0', N'自訂問題', N'單選方塊', N'question2', 0, N'question1-answer1;question1-answer2;question3-answer3', CAST(N'2022-04-29T19:53:34.880' AS DateTime), CAST(N'2022-04-29T19:53:34.880' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'91742aeb-9d77-4448-95fd-94277e809d6a', N'6efbd6d5-165d-485d-9cc0-53eea3dd1f4c', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:24:04.873' AS DateTime), CAST(N'2022-04-12T20:24:04.873' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'932dea53-f909-44f2-9182-9fcdab5c2605', N'f8297932-0149-4f0f-926d-e89492b92cb0', N'常用問題', N'單選方塊', N'cQuestion1', 1, N'cQuestion1-answer1;cQuestion1-answer2;cQuestion1-answer3', CAST(N'2022-04-25T04:35:15.650' AS DateTime), CAST(N'2022-04-25T04:35:15.650' AS DateTime), N'c54db6cd-4cc8-4930-b406-5ff17a14826c')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'9eff871d-1aec-4fca-9571-a2ba32991fd5', N'b32a91af-e4ca-48c9-a2e2-3301fa81d5a6', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:29:52.337' AS DateTime), CAST(N'2022-04-12T20:29:52.337' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'文字', N'question3', 1, N'question3-answer1;question3-answer2;question3-answer3', CAST(N'2022-04-14T18:09:53.097' AS DateTime), CAST(N'2022-04-14T18:09:53.097' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'複選方塊', N'question2', 1, N'question2-answer1;question2-answer2;question2-answer3', CAST(N'2022-04-14T17:09:53.097' AS DateTime), CAST(N'2022-04-14T17:09:53.097' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'9e61d9c4-bd3c-4700-9302-b35877bdb5f6', N'3c1217e1-29d9-4348-a191-0811a36c7ef9', N'常用問題', N'單選方塊', N'cQuestion1', 0, N'cQuestion1-answer1;cQuestion1-answer2;cQuestion1-answer3', CAST(N'2022-05-08T15:59:37.117' AS DateTime), CAST(N'2022-05-08T16:00:21.633' AS DateTime), N'1cab573c-3daf-4549-960d-50bb03a1f611')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'588f3434-effa-4c8e-b14e-b5d67687fdcc', N'd9a7c5d5-41c2-4d0b-8266-56887ea101e0', N'自訂問題', N'複選方塊', N'question3', 0, N'question3-answer1;question3-answer2;question3-answer3', CAST(N'2022-04-30T03:52:36.517' AS DateTime), CAST(N'2022-05-01T00:21:19.100' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'e2067991-0787-4bad-a02e-bb8b3d73c640', N'61e972a5-ec4f-45e7-9161-c3d2fc65a0da', N'常用問題', N'複選方塊', N'cQuestion4', 0, N'cQuestion4-answer1;cQuestion4-answer2;cQuestion4-answer3', CAST(N'2022-04-25T04:56:35.040' AS DateTime), CAST(N'2022-04-25T05:25:54.873' AS DateTime), N'2a59fd0f-926d-47f8-9bfe-2e1dd9eabc05')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'b2802fb8-3c7f-4d56-9fc3-bdf625c712d7', N'513d0355-333a-4a55-a00c-c9327a26113f', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:28:02.777' AS DateTime), CAST(N'2022-04-12T20:28:02.777' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'3c93c956-624d-47ff-b052-bebd39ed182b', N'ef71f18e-4cd0-41f1-9cd4-dcc19f63faf6', N'自訂問題', N'單選方塊', N't', 1, N't', CAST(N'2022-04-12T21:59:41.447' AS DateTime), CAST(N'2022-04-12T21:59:41.447' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'0e727b56-e9b5-434c-a295-cd527f110ab4', N'e9af1926-5442-4290-b3b6-e2725a300e42', N'自訂問題', N'單選方塊', N't', 0, N't', CAST(N'2022-04-12T20:29:32.720' AS DateTime), CAST(N'2022-04-12T20:29:32.720' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'3ec9bba7-68b4-43c7-a2e2-cf384c372094', N'3c1217e1-29d9-4348-a191-0811a36c7ef9', N'常用問題', N'複選方塊', N'cQuestion2', 1, N'cQuestion2-answer1;cQuestion2-answer2;cQuestion2-answer3', CAST(N'2022-05-08T15:59:51.087' AS DateTime), CAST(N'2022-05-08T16:00:34.250' AS DateTime), N'1cab573c-3daf-4549-960d-50bb03a1f611')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'60cbf2ef-b6a0-4e3a-ad77-cfc36c991dbc', N'f8297932-0149-4f0f-926d-e89492b92cb0', N'常用問題', N'文字', N'cQuestion2', 1, N'cQuestion2-answer1;cQuestion2-answer2;cQuestion2-answer3', CAST(N'2022-04-25T06:39:58.110' AS DateTime), CAST(N'2022-04-25T06:39:58.110' AS DateTime), N'c54db6cd-4cc8-4930-b406-5ff17a14826c')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'c8a18cca-6d5f-4f18-88ad-d50f49ae49df', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'單選方塊', N'question4', 0, N'question4-answer1;question4-answer2;question4-answer3', CAST(N'2022-04-13T17:09:53.097' AS DateTime), CAST(N'2022-04-13T17:09:53.097' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'自訂問題', N'複選方塊', N'question5', 0, N'question5-answer1;question5-answer2;question5-answer3', CAST(N'2022-04-14T17:09:53.097' AS DateTime), CAST(N'2022-04-14T17:09:53.097' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'4190b55e-edf0-4d1b-bea1-e17c7869b570', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'自訂問題', N'文字', N'question5', 0, N'question5-answer1;question5-answer2;question5-answer3', CAST(N'2022-04-20T16:43:31.823' AS DateTime), CAST(N'2022-04-20T16:43:31.823' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'6b2ab15d-85ac-4650-97ea-e22c550f39c0', N'd9a7c5d5-41c2-4d0b-8266-56887ea101e0', N'自訂問題', N'單選方塊', N'question1', 0, N'question1-answer1;question1-answer2;question1-answer3', CAST(N'2022-04-30T03:52:10.133' AS DateTime), CAST(N'2022-04-30T03:52:10.133' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'94d802df-18ac-4e5d-a65a-ec65ca41dec9', N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'自訂問題', N'單選方塊', N'question4', 1, N'question4-answer1', CAST(N'2022-05-08T12:18:12.883' AS DateTime), CAST(N'2022-05-08T12:18:12.883' AS DateTime), NULL)
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'bf5aa0ed-b870-4702-bd0c-edf749bc6569', N'8b975e5d-40ea-4670-b7b9-b5c4df3d6913', N'常用問題', N'單選方塊', N'cQuestion1', 0, N'cQuestion1-answer1;cQuestion1-answer2;cQuestion1-answer3', CAST(N'2022-05-11T17:28:51.957' AS DateTime), CAST(N'2022-05-11T17:28:51.957' AS DateTime), N'a5468859-53b8-446a-beec-68c8a8e30b17')
GO
INSERT [dbo].[Questions] ([QuestionID], [QuestionnaireID], [QuestionCategory], [QuestionTyping], [QuestionName], [QuestionRequired], [QuestionAnswer], [CreateDate], [UpdateDate], [CommonQuestionID]) VALUES (N'4c8592a6-996f-4697-a0ce-f4165bec24df', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'自訂問題', N'複選方塊', N'question4', 0, N'question4-answer1;question4-answer2;question4-answer3', CAST(N'2022-04-20T16:43:01.280' AS DateTime), CAST(N'2022-04-20T16:43:01.280' AS DateTime), NULL)
GO
INSERT [dbo].[Typings] ([TypingID], [TypingName]) VALUES (N'd7a18a03-515f-4981-b03e-6ba88a072397', N'複選方塊')
GO
INSERT [dbo].[Typings] ([TypingID], [TypingName]) VALUES (N'0fd9d755-1ca3-4b5f-a17a-9930c88cec06', N'文字')
GO
INSERT [dbo].[Typings] ([TypingID], [TypingName]) VALUES (N'b6f43a7b-962d-4d07-af59-bfea056beee5', N'單選方塊')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'複選方塊', 3, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'837a291b-980b-4c9d-9512-067fa443a464', N'文字', 1, N't61')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'837a291b-980b-4c9d-9512-067fa443a464', N'文字', 2, N't62')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'837a291b-980b-4c9d-9512-067fa443a464', N'文字', 3, N't63')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'986cec09-0d92-49cd-8f55-7b50aaab2198', N'單選方塊', 3, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 1, N't31')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 2, N't32')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 3, N't33')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'85529005-5119-4200-bbf1-6f35c949e577', N'c8a18cca-6d5f-4f18-88ad-d50f49ae49df', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'760b89a2-fbf0-4490-a157-d6f85d733e1a', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'760b89a2-fbf0-4490-a157-d6f85d733e1a', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'760b89a2-fbf0-4490-a157-d6f85d733e1a', N'986cec09-0d92-49cd-8f55-7b50aaab2198', N'單選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'760b89a2-fbf0-4490-a157-d6f85d733e1a', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 1, N't31')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'760b89a2-fbf0-4490-a157-d6f85d733e1a', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 2, N't32')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'760b89a2-fbf0-4490-a157-d6f85d733e1a', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 3, N't33')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'760b89a2-fbf0-4490-a157-d6f85d733e1a', N'c8a18cca-6d5f-4f18-88ad-d50f49ae49df', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'68b9ad5b-8354-4ac1-ae70-192067f11de5', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'68b9ad5b-8354-4ac1-ae70-192067f11de5', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'68b9ad5b-8354-4ac1-ae70-192067f11de5', N'986cec09-0d92-49cd-8f55-7b50aaab2198', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'68b9ad5b-8354-4ac1-ae70-192067f11de5', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 1, N't31')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'68b9ad5b-8354-4ac1-ae70-192067f11de5', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 2, N't32')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'68b9ad5b-8354-4ac1-ae70-192067f11de5', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 3, N't33')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 1, N't21')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 2, N't22')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 3, N't23')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'4190b55e-edf0-4d1b-bea1-e17c7869b570', N'文字', 1, N't51')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'4190b55e-edf0-4d1b-bea1-e17c7869b570', N'文字', 2, N't52')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'4190b55e-edf0-4d1b-bea1-e17c7869b570', N'文字', 3, N't53')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'45e57fc1-f429-4083-9183-80937884374e', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'45e57fc1-f429-4083-9183-80937884374e', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'4c8592a6-996f-4697-a0ce-f4165bec24df', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'4c8592a6-996f-4697-a0ce-f4165bec24df', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'a8703c65-e2f7-4339-bf9f-7fb556978752', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'ca7f29da-d590-409b-995e-3cef142399bc', N'c1615277-65d6-4cd4-941d-663152ea348f', N'單選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 1, N't21')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 2, N't22')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 3, N't23')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'4190b55e-edf0-4d1b-bea1-e17c7869b570', N'文字', 1, N't51')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'4190b55e-edf0-4d1b-bea1-e17c7869b570', N'文字', 2, N't52')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'4190b55e-edf0-4d1b-bea1-e17c7869b570', N'文字', 3, N't53')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'45e57fc1-f429-4083-9183-80937884374e', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'45e57fc1-f429-4083-9183-80937884374e', N'複選方塊', 3, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'c1615277-65d6-4cd4-941d-663152ea348f', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'f5125ae5-63c5-4040-918a-95a7833a7bf3', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 1, N't21')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'f5125ae5-63c5-4040-918a-95a7833a7bf3', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 2, N't22')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'f5125ae5-63c5-4040-918a-95a7833a7bf3', N'1f84dba9-b7fd-4a4e-a1af-6221c78095fc', N'文字', 3, N't23')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'f5125ae5-63c5-4040-918a-95a7833a7bf3', N'45e57fc1-f429-4083-9183-80937884374e', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'f5125ae5-63c5-4040-918a-95a7833a7bf3', N'a8703c65-e2f7-4339-bf9f-7fb556978752', N'單選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'f5125ae5-63c5-4040-918a-95a7833a7bf3', N'c1615277-65d6-4cd4-941d-663152ea348f', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'986cec09-0d92-49cd-8f55-7b50aaab2198', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 1, N't31')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 2, N't32')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 3, N't33')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'c8a18cca-6d5f-4f18-88ad-d50f49ae49df', N'單選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'0432fdd9-dfd8-4f3e-8a7c-dd7995aadae9', N'複選方塊', 3, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'865f4317-cfd8-4fba-97f0-b13e28140dd5', N'複選方塊', 3, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'986cec09-0d92-49cd-8f55-7b50aaab2198', N'單選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 1, N't31')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 2, N't32')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'ab7a8240-aaa6-43de-a2a0-a3f54128b8b6', N'文字', 3, N't33')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'c8a18cca-6d5f-4f18-88ad-d50f49ae49df', N'單選方塊', 3, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'79f0e00c-132d-45b2-93b8-27ce498e574b', N'94d802df-18ac-4e5d-a65a-ec65ca41dec9', N'單選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'79f0e00c-132d-45b2-93b8-27ce498e574b', N'ee0635e5-ad51-472e-9277-8c31105007d5', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'79f0e00c-132d-45b2-93b8-27ce498e574b', N'ee0635e5-ad51-472e-9277-8c31105007d5', N'複選方塊', 2, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'51d52bd8-9c5d-48f9-8927-a652ffae30bb', N'94d802df-18ac-4e5d-a65a-ec65ca41dec9', N'單選方塊', 1, N'on')
GO
INSERT [dbo].[UserAnswers] ([QuestionnaireID], [UserID], [QuestionID], [QuestionTyping], [AnswerNum], [Answer]) VALUES (N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'51d52bd8-9c5d-48f9-8927-a652ffae30bb', N'ee0635e5-ad51-472e-9277-8c31105007d5', N'複選方塊', 1, N'on')
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'68b9ad5b-8354-4ac1-ae70-192067f11de5', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'user3', N'0123456787', N'tT3@eE.com', 13, CAST(N'2022-04-20T21:36:29.117' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'79f0e00c-132d-45b2-93b8-27ce498e574b', N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'user1', N'0987654321', N'tt@TT.com', 33, CAST(N'2022-05-08T12:19:22.730' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'520c08c9-b2a6-413b-954d-38cc2a72b567', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'user5', N'0987654322', N'tt5@TT.com', 44, CAST(N'2022-05-07T15:24:21.433' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'ca7f29da-d590-409b-995e-3cef142399bc', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'tt', N'0123456789', N'tT@eE.com', 11, CAST(N'2022-04-20T21:40:46.407' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'85529005-5119-4200-bbf1-6f35c949e577', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'user1', N'0123456789', N'tT@eE.com', 11, CAST(N'2022-04-20T21:34:42.850' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'f5125ae5-63c5-4040-918a-95a7833a7bf3', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'tt3', N'0123456787', N'tT3@eE.com', 13, CAST(N'2022-04-20T21:42:17.657' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'bbc019d4-b822-4ade-9d14-a54ca03bafe2', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'user4', N'0987654321', N'tt4@TT.com', 15, CAST(N'2022-05-07T15:21:48.850' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'51d52bd8-9c5d-48f9-8927-a652ffae30bb', N'8cb0a0c2-e863-439e-b7e5-f86ac0af153b', N'user2', N'0987654322', N'tt2@TT.com', 22, CAST(N'2022-05-08T12:20:07.217' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'620a64f9-cbbd-4212-b28b-b1c25979f427', N'f25adc80-5352-4977-aaee-74b3ab7cde28', N'tt2', N'0123456788', N'tT2@eE.com', 12, CAST(N'2022-04-20T21:41:43.560' AS DateTime))
GO
INSERT [dbo].[Users] ([UserID], [QuestionnaireID], [UserName], [Phone], [Email], [Age], [AnswerDate]) VALUES (N'760b89a2-fbf0-4490-a157-d6f85d733e1a', N'2fcc62d5-7d7a-4a00-9149-b18e97853a71', N'user2', N'0123456788', N'tT2@eE.com', 12, CAST(N'2022-04-20T21:35:48.957' AS DateTime))
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_CategoryID]  DEFAULT (newid()) FOR [CategoryID]
GO
ALTER TABLE [dbo].[Typings] ADD  CONSTRAINT [DF_Types_TypeID]  DEFAULT (newid()) FOR [TypingID]
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

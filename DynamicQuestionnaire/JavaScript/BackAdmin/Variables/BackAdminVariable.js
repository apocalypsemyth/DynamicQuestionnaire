const FAILED = "FAILED";
const NULL = "NULL";
const SUCCESSED = "SUCCESSED";
const PAGESIZE = 4;

let errorMessageOfRetry = "發生錯誤，請再嘗試。";
let errorMessageOfAjax = "通訊失敗，請聯絡管理員。";
let emptyMessageOfQuestionList = "<p>尚未有資料</p>";
let emptyMessageOfUserListOrStatistics = "<p>尚未有使用者的回答。</p>";
let commonQuestionOfCategoryName = "常用問題";
let customizedQuestionOfCategoryName = "自訂問題";
let showState = "show";
let hideState = "hide";
let setState = "set";
let notSetState = "notSet";

// Session name of QuestionnaireDetail
let activeTab = "activeTab";
let currentCommonQuestionOfCategoryNameValue = "currentCommonQuestionOfCategoryNameValue";
let currentCommonQuestionOfCategoryNameShowState = "currentCommonQuestionOfCategoryNameShowState";
let currentSetCommonQuestionOnQuestionnaireState = "currentSetCommonQuestionOnQuestionnaire";
let currentQuestionListTable = "currentQuestionListTable";
let currentUserList = "currentUserList";
let currentUserListShowState = "currentUserListShowState";
let currentUserListPager = "currentUserListPager";
let currentUserAnswer = "currentUserAnswer";
let currentUserAnswerShowState = "currentUserAnswerShowState";
let currentStatistics = "currentStatistics";

// Controls of QuestionnaireDetail
let txtCaption = "input[id*=txtCaption]";
let txtDescription = "textarea[id*=txtDescription]";
let txtStartDate = "input[id*=txtStartDate]";
let txtEndDate = "input[id*=txtEndDate]";
let ckbIsEnable = "input[id*=ckbIsEnable]";
let selectCategoryList = "select[id*=ddlCategoryList]";
let selectTypingList = "select[id*=ddlTypingList]";
let txtQuestionName = "input[id*=txtQuestionName]";
let txtQuestionAnswer = "input[id*=txtQuestionAnswer]";
let ckbQuestionRequired = "input[id*=ckbQuestionRequired]";
let btnAddQuestion = "button[id=btnAddQuestion]";
let btnDeleteQuestion = "button[id=btnDeleteQuestion]";
let divQuestionListContainer = "#divQuestionListContainer";
let btnExportAndDownloadDataToCSVContainer = "#btnExportAndDownloadDataToCSVContainer";
let divUserListContainer = "#divUserListContainer";
let divUserListPagerContainer = "#divUserListPagerContainer";
let divUserAnswerContainer = "#divUserAnswerContainer";
let divStatisticsContainer = "#divStatisticsContainer";

// Session name of CommonQuestion
let currentQuestionListOfCommonQuestionTable = "currentQuestionListOfCommonQuestionTable";

// Controls of CommonQuestion
let divQuestionListOfCommonQuestionContainer = "#divQuestionListOfCommonQuestionContainer";
let btnAddQuestionOfCommonQuestion = "button[id=btnAddQuestionOfCommonQuestion]";
let btnDeleteQuestionOfCommonQuestion = "button[id=btnDeleteQuestionOfCommonQuestion]";
// Global variables
const FAILED = "FAILED";
const NULL = "NULL";
const SUCCESSED = "SUCCESSED";
const PAGESIZE = 4;
const SINGLE_SELECT = "單選方塊";
const MULTIPLE_SELECT = "複選方塊";
const TEXT = "文字";

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
let enabledState = "enabled";
let disabledState = "disabled";
let startPercent = "30%";
let endPercent = "100%";
let fadeOutDuration = 100;

// Global functions
var SetContainerSession = function (strSelector, strSessionName) {
    let strHtml = $(strSelector).html();
    sessionStorage.setItem(strSessionName, strHtml);
}
var ResetQuestionnaireAndCommonQuestionDetailSession = function () {
    $.ajax({
        url: "/API/BackAdmin/QuestionnaireAndCommonQuestionDetailDataHandler.ashx?Action=RESET_QUESTIONNAIRE_AND_COMMONQUESTIONDETAIL_SESSION",
        method: "GET",
        success: function (strMsg) {
            if (strMsg === SUCCESSED) {
            }
        },
        error: function (msg) {
            console.log(msg);
            alert(errorMessageOfAjax);
        }
    });
}
function ResetListCheckedForServer() {
    $("input[type=checkbox]").prop("checked", false);
}

// Controls of Components of ucLoadingProgressBar
let loadingProgressBarContainer = "#loadingProgressBarContainer";
let loadingProgressBar = "#loadingProgressBar";

// Session name of QuestionnaireDetail
let activeTab = "activeTab";
let currentCommonQuestionOfCategoryNameValue = "currentCommonQuestionOfCategoryNameValue";
let currentCommonQuestionOfCategoryNameShowState = "currentCommonQuestionOfCategoryNameShowState";
let currentSetCommonQuestionOnQuestionnaireState = "currentSetCommonQuestionOnQuestionnaire";
let currentQuestionListTable = "currentQuestionListTable";
let currentQuestionListItsControlsDisabledState = "currentQuestionListItsControlsDisabledState";
let currentUserList = "currentUserList";
let currentUserListShowState = "currentUserListShowState";
let currentUserListPager = "currentUserListPager";
let currentUserAnswer = "currentUserAnswer";
let currentUserAnswerShowState = "currentUserAnswerShowState";
let currentStatistics = "currentStatistics";

/** Controls of QuestionnaireDetail or CommonQuestionDetail */
let selectCategoryList = "select[id*=ddlCategoryList]";
let selectTypingList = "select[id*=ddlTypingList]";

// Controls of QuestionnaireDetail
let txtCaption = "input[id*=txtCaption]";
let txtDescription = "textarea[id*=txtDescription]";
let txtStartDate = "input[id*=txtStartDate]";
let txtEndDate = "input[id*=txtEndDate]";
let ckbIsEnable = "input[id*=ckbIsEnable]";
let txtQuestionName = "input[id*=txtQuestionName]";
let txtQuestionAnswer = "input[id*=txtQuestionAnswer]";
let ckbQuestionRequired = "input[id*=ckbQuestionRequired]";
let btnAddQuestion = "input[id*=btnAddQuestion]";
let btnDeleteQuestion = "input[id*=btnDeleteQuestion]";
let divQuestionListContainer = "#divQuestionListContainer";
let btnExportAndDownloadDataToCSVContainer = "#btnExportAndDownloadDataToCSVContainer";
let divUserListContainer = "#divUserListContainer";
let divUserListPagerContainer = "#divUserListPagerContainer";
let divUserAnswerContainer = "#divUserAnswerContainer";
let divStatisticsContainer = "#divStatisticsContainer";

// Session name of CommonQuestion
let currentQuestionListOfCommonQuestionTable = "currentQuestionListOfCommonQuestionTable";

// Controls of CommonQuestion
let txtCommonQuestionName = "input[id*=txtCommonQuestionName]";
let txtQuestionNameOfCommonQuestion = "input[id*=txtQuestionNameOfCommonQuestion]";
let txtQuestionAnswerOfCommonQuestion = "input[id*=txtQuestionAnswerOfCommonQuestion]";
let ckbQuestionRequiredOfCommonQuestion = "input[id*=ckbQuestionRequiredOfCommonQuestion]";
let divQuestionListOfCommonQuestionContainer = "#divQuestionListOfCommonQuestionContainer";
let btnAddQuestionOfCommonQuestion = "button[id=btnAddQuestionOfCommonQuestion]";
let btnDeleteQuestionOfCommonQuestion = "button[id=btnDeleteQuestionOfCommonQuestion]";
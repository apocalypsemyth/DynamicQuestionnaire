$(document).ready(function () {
    let currentUrl = window.location.href;
    let isQuestionnaireDetail = currentUrl.indexOf("QuestionnaireDetail.aspx") !== -1;
    let isCommonQuestionDetail = currentUrl.indexOf("CommonQuestionDetail.aspx") !== -1;

    if (!(isQuestionnaireDetail || isCommonQuestionDetail))
        ResetQuestionnaireAndCommonQuestionDetailSession();
});
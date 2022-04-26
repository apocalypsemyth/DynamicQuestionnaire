$(document).ready(function () {
    if (window.location.href.indexOf("QuestionnaireDetail.aspx") === -1) {
        if (sessionStorage.getItem(activeTab) != null)
            sessionStorage.removeItem(activeTab);

        if (sessionStorage.getItem(currentQuestionListTable) != null)
            sessionStorage.removeItem(currentQuestionListTable);

        if (sessionStorage.getItem(currentUserList) != null);
            sessionStorage.removeItem(currentUserList);

        if (sessionStorage.getItem(currentUserListShowState) != null)
            sessionStorage.removeItem(currentUserListShowState);

        if (sessionStorage.getItem(currentUserAnswer) != null);
            sessionStorage.removeItem(currentUserAnswer);

        if (sessionStorage.getItem(currentUserAnswerShowState) != null);
            sessionStorage.removeItem(currentUserAnswerShowState);

        if (sessionStorage.getItem(currentUserListPager) != null);
            sessionStorage.removeItem(currentUserListPager);

        if (sessionStorage.getItem(currentStatistics) != null);
            sessionStorage.removeItem(currentStatistics);
    }
    else {
        let currentActiveTab = sessionStorage.getItem(activeTab);
        // question
        let strQuestionListHtml = sessionStorage.getItem(currentQuestionListTable);
        if (strQuestionListHtml) 
            $(divQuestionListContainer).html(strQuestionListHtml);
        // question-info userList
        let strUserListHtml = sessionStorage.getItem(currentUserList);
        let strUserListShowState = sessionStorage.getItem(currentUserListShowState)
        let strUserListPagerHtml = sessionStorage.getItem(currentUserListPager);
        if (strUserListHtml && currentActiveTab === "#question-info") {
            $(divQuestionListContainer).empty();

            if (strUserListShowState === showState) {
                $(btnExportAndDownloadDataToCSV).show();
                $(divUserListContainer).html(strUserListHtml);
                $(divUserListPagerContainer).html(strUserListPagerHtml);
            }
            else {
                $(btnExportAndDownloadDataToCSV).hide();
                $(divUserListContainer).empty();
                $(divUserListPagerContainer).empty();
            }
        }
        else if (strUserListHtml) {
            if (strUserListShowState === showState) {
                $(btnExportAndDownloadDataToCSV).show();
                $(divUserListContainer).html(strUserListHtml);
                $(divUserListPagerContainer).html(strUserListPagerHtml);
            }
            else {
                $(btnExportAndDownloadDataToCSV).hide();
                $(divUserListContainer).empty();
                $(divUserListPagerContainer).empty();
            }
        }
        // question-info userAnswer
        let strUserAnswerHtml = sessionStorage.getItem(currentUserAnswer);
        let strUserAnswerHtmlShowState = sessionStorage.getItem(currentUserAnswerShowState);
        if (strUserAnswerHtml && currentActiveTab === "#question-info") {
            $(divQuestionListContainer).empty();

            if (strUserAnswerHtmlShowState === showState) 
                $(divUserAnswerContainer).html(strUserAnswerHtml);
            else
                $(divUserAnswerContainer).empty();
        }
        else if (strUserAnswerHtml) {
            if (strUserAnswerHtmlShowState === showState) 
                $(divUserAnswerContainer).html(strUserAnswerHtml);
            else
                $(divUserAnswerContainer).empty();
        }
        // statistics
        let strStatisticsHtml = sessionStorage.getItem(currentStatistics);
        if (strStatisticsHtml)
            $(divStatisticsContainer).html(strStatisticsHtml);

        let currentQueryString = window.location.search;
        let isExistQueryString = currentQueryString.indexOf("?ID=") !== -1;
        let strQuestionnaireID = isExistQueryString ? currentQueryString.split("?ID=")[1] : "";
        if (isExistQueryString) {
            GetQuestionnaire(strQuestionnaireID);
            GetStatistics(strQuestionnaireID);

            if (!strUserListHtml)
                GetUserList(strQuestionnaireID);
            else if (strUserListHtml && strUserListHtml === showState)
                GetUserList(strQuestionnaireID);
        }
        else {
            $(divUserListContainer).html(emptyMessageOfUserListOrStatistics);
            $(divUserListPagerContainer).empty();
            $(divStatisticsContainer).html(emptyMessageOfUserListOrStatistics);
        }
        GetQuestionList(strQuestionnaireID);

        $("#ulQuestionnaireDetailTabs li a[data-bs-toggle='tab']").on("show.bs.tab", function () {
            sessionStorage.setItem(activeTab, $(this).attr("href"));
        });
        let strActiveTab = sessionStorage.getItem(activeTab);
        if (strActiveTab) {
            $("#ulQuestionnaireDetailTabs a[href='" + strActiveTab + "']").tab("show");
        }
        if (strActiveTab === "#question") {
            $(divQuestionListContainer).html(strQuestionListHtml);
        }

        $("select[id*=ddlCategoryList]").click(function (e) {
            e.preventDefault();
            let strSelectedCategoryText = $(this).find(":selected").text();

            if (strSelectedCategoryText == "自訂問題")
                return;

            let strSelectedCategoryID = $(this).val();
            SetQuestionListOfCommonQuestionOnQuestionnaire(strSelectedCategoryID);
        });
        $("button[id=btnAddQuestion]").click(function (e) {
            e.preventDefault();

            let objQuestionnaire = GetQuestionnaireInputs();
            let isValidQuestionnaire = CheckQuestionnaireInputs(objQuestionnaire);
            if (typeof isValidQuestionnaire === "string") {
                alert(isValidQuestionnaire);
                return;
            }

            if (window.location.search.indexOf("?ID=") !== -1) {
                UpdateQuestionnaire(objQuestionnaire);

                let objQuestion = GetQuestionInputs();
                let isValidQuestion = CheckQuestionInputs(objQuestion);
                if (typeof isValidQuestion === "string") {
                    alert(isValidQuestion);
                    return;
                }

                CreateQuestion(objQuestion);
            }
            else 
                CreateQuestionnaire(objQuestionnaire);
        });
        $("button[id=btnDeleteQuestion]").click(function (e) {
            e.preventDefault();

            let arrCheckedQuestionID = [];
            $("#divQuestionListContainer table tbody tr td input[type='checkbox']:checked")
                .each(function () {
                    arrCheckedQuestionID.push($(this).attr("id"));
                });

            DeleteQuestionList(arrCheckedQuestionID.join());
        });

        $(document).on("click", "a.currentEditQuestion[id*=aLinkEditQuestion]", function (e) {
            e.preventDefault();

            if (window.location.search.indexOf("?ID=") === -1) {
                alert("請先新增後，再編輯");
                return;
            }

            $(this).toggleClass("currentEditQuestion");

            let objQuestion = GetQuestionInputs();
            let isValidQuestion = CheckQuestionInputs(objQuestion);
            if (typeof isValidQuestion === "string") {
                alert(isValidQuestion);
                return;
            }

            let aLinkHref = $(this).attr("href");
            let strQuestionID = aLinkHref.split('?QuestionID=')[1];
            objQuestion.clickedQuestionID = strQuestionID;
            UpdateQuestion(objQuestion);
        });
        $(document).on("click", "a:not(.currentEditQuestion)[id*=aLinkEditQuestion]", function (e) {
            e.preventDefault();

            if (window.location.search.indexOf("?ID=") === -1) {
                alert("請先新增後，再編輯");
                return;
            }

            $("#divQuestionListContainer table tbody tr td a.currentEditQuestion[id*=aLinkEditQuestion]").removeClass("currentEditQuestion");
            $(this).toggleClass("currentEditQuestion");

            let aLinkHref = $(this).attr("href");
            let strQuestionID = aLinkHref.split("?QuestionID=")[1];
            ShowToUpdateQuestion(strQuestionID);
        });

        $(document).on("click", "a[id*=aLinkUserAnswer]", function (e) {
            e.preventDefault();

            let aLinkHref = $(this).attr("href");
            let strUserID = aLinkHref.split("?UserID=")[1];
            let objQuestionnaireAndUserID = {
                "questionnaireID": strQuestionnaireID,
                "userID": strUserID,
            };
            GetUserAnswer(objQuestionnaireAndUserID);
        });
        $(document).on("click", "a[id*=aLinkUserListPager]", function (e) {
            e.preventDefault();

            let aLinkHref = $(this).attr("href");
            let strIndex = aLinkHref.split("?Index=")[1];
            let objQuestionnaireIDAndIndex = {
                "questionnaireID": strQuestionnaireID,
                "index": strIndex,
            };
            UpdateUserList(objQuestionnaireIDAndIndex);
        });
        $(document).on("click", "button[id=btnBackToUserList]", function (e) {
            e.preventDefault();

            $(btnExportAndDownloadDataToCSV).show();
            $(divUserAnswerContainer).empty();
            $(divUserListContainer).html(strUserListHtml);
            $(divUserListPagerContainer).html(strUserListPagerHtml);
            SetUserAnswerShowStateSession(hideState);
            SetUserListShowStateSession(showState);
        });
    }
});
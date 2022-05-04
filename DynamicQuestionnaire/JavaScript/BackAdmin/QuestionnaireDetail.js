﻿$(document).ready(function () {
    if (window.location.href.indexOf("QuestionnaireDetail.aspx") === -1) {
        if (sessionStorage.getItem(activeTab) != null)
            sessionStorage.removeItem(activeTab);

        if (sessionStorage.getItem(currentCommonQuestionOfCategoryNameShowState) != null)
            sessionStorage.removeItem(currentCommonQuestionOfCategoryNameShowState);

        if (sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState) != null)
            sessionStorage.removeItem(currentSetCommonQuestionOnQuestionnaireState);

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
        if (strQuestionListHtml) {
            $(divQuestionListContainer).empty();
            $(divQuestionListContainer).html(strQuestionListHtml);
        }
        // question-info userList
        let strUserListHtml = sessionStorage.getItem(currentUserList);
        let strUserListShowState = sessionStorage.getItem(currentUserListShowState);
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

        let strCurrentCommonQuestionOfCategoryNameShowState =
            sessionStorage.getItem(currentCommonQuestionOfCategoryNameShowState);
        if (strCurrentCommonQuestionOfCategoryNameShowState === showState)
            $(selectCategoryList + " option[value='" + commonQuestionOfCategoryNameValue + "']").show();
        else
            $(selectCategoryList + " option[value='" + commonQuestionOfCategoryNameValue + "']").hide();

        $(selectCategoryList).click(function (e) {
            e.preventDefault();

            let strSelectedCategoryText = $(this).find(":selected").text();

            let strCurrentSetCommonQuestionOnQuestionnaireState =
                sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
            if (strCurrentSetCommonQuestionOnQuestionnaireState === settedState) {
                if (strSelectedCategoryText === customizedQuestionOfCategoryName) {
                    let isSetCustomizedQuestion =
                        confirm("如果已經選擇常用問題，\n再次選擇自訂問題，\n會將先前的常用問題全部移除，\n請問仍要繼續嗎？");
                    if (isSetCustomizedQuestion) 
                        DeleteSettedQuestionListOfCommonQuestionOnQuestionnaire();
                }
            }
            else {
                if (strSelectedCategoryText === customizedQuestionOfCategoryName
                    || strSelectedCategoryText === commonQuestionOfCategoryName)
                    return;

                let strSelectedCategoryID = $(this).val();
                SetQuestionListOfCommonQuestionOnQuestionnaire(strSelectedCategoryID);
            }
        });
        $(btnAddQuestion).click(function (e) {
            e.preventDefault();
            
            let objQuestionnaire = GetQuestionnaireInputs();
            let isValidQuestionnaire = CheckQuestionnaireInputs(objQuestionnaire);
            if (typeof isValidQuestionnaire === "string") {
                alert(isValidQuestionnaire);
                return;
            }

            let objQuestion = GetQuestionInputs();
            let isValidQuestion = CheckQuestionInputs(objQuestion);
            if (typeof isValidQuestion === "string") {
                alert(isValidQuestion);
                return;
            }

            let strCurrentSetCommonQuestionOnQuestionnaireState =
                sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
            if (strCurrentSetCommonQuestionOnQuestionnaireState === settedState) {
                let isToModifyCommonQuestion =
                    confirm("如果對常用問題進行任何增刪修，\n其將成為自訂問題，\n請問仍要繼續嗎？");
                if (!isToModifyCommonQuestion)
                    return;
                else {
                    $(selectCategoryList + " option[value='" + commonQuestionOfCategoryNameValue + "']")
                        .hide();
                    SetContainerShowStateSession(currentCommonQuestionOfCategoryNameShowState, hideState);

                    $(selectCategoryList + " option").filter(function () {
                        return $(this).text() == customizedQuestionOfCategoryName;
                    }).prop('selected', true);
                    objQuestion.questionCategory = customizedQuestionOfCategoryName;

                    SetElementCurrentStateSession(
                        currentSetCommonQuestionOnQuestionnaireState,
                        notSettedState
                    );
                }
            }

            if (window.location.search.indexOf("?ID=") !== -1) {
                let btnHref = $(this).attr("href");

                if (btnHref) {
                    let strQuestionID = $(this).attr("href");
                    objQuestion.clickedQuestionID = strQuestionID;
                    UpdateQuestion(objQuestion);
                }
                else {
                    UpdateQuestionnaire(objQuestionnaire);
                    CreateQuestion(objQuestion);
                }
            }
            else 
                CreateQuestionnaire(objQuestionnaire);
        });
        $(btnDeleteQuestion).click(function (e) {
            e.preventDefault();

            let arrCheckedQuestionID = [];
            $("#divQuestionListContainer table tbody tr td input[type='checkbox']:checked")
                .each(function () {
                    arrCheckedQuestionID.push($(this).attr("id"));
                });
            if (arrCheckedQuestionID.length === 0) {
                alert("請選擇要刪除的問題。");
                return;
            }

            let strCurrentSetCommonQuestionOnQuestionnaireState =
                sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
            if (strCurrentSetCommonQuestionOnQuestionnaireState === settedState) {
                let isToModifyCommonQuestion = 
                    confirm("如果對常用問題進行任何增刪修，\n其將成為自訂問題，\n請問仍要繼續嗎？");
                if (!isToModifyCommonQuestion)
                    return;
                else {
                    $(selectCategoryList + " option[value='" + commonQuestionOfCategoryNameValue + "']")
                        .hide();
                    SetContainerShowStateSession(currentCommonQuestionOfCategoryNameShowState, hideState);

                    $(selectCategoryList + " option").filter(function () {
                        return $(this).text() == customizedQuestionOfCategoryName;
                    }).prop('selected', true);

                    SetElementCurrentStateSession(
                        currentSetCommonQuestionOnQuestionnaireState,
                        notSettedState
                    );
                }
            }

            DeleteQuestionList(arrCheckedQuestionID.join());
        });

        $(document).on("click", "a[id*=aLinkEditQuestion]", function (e) {
            e.preventDefault();

            if (window.location.search.indexOf("?ID=") === -1) {
                alert("請先新增後，再編輯。");
                return;
            }

            let aLinkHref = $(this).attr("href");
            let strQuestionID = aLinkHref.split("?QuestionID=")[1];
            $(btnAddQuestion).attr("href", strQuestionID);
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

            $(divUserAnswerContainer).empty();

            $(btnExportAndDownloadDataToCSV).show();
            let strUserListHtml = sessionStorage.getItem(currentUserList);
            let strUserListPagerHtml = sessionStorage.getItem(currentUserListPager);
            $(divUserListContainer).html(strUserListHtml);
            $(divUserListPagerContainer).html(strUserListPagerHtml);

            SetContainerShowStateSession(currentUserAnswerShowState, hideState);
            SetContainerShowStateSession(currentUserListShowState, showState);
        });
    }
});
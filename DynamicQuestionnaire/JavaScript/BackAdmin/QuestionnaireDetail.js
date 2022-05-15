$(document).ready(function () {
    if (window.location.href.indexOf("QuestionnaireDetail.aspx") === -1) {
        sessionStorage.removeItem(activeTab);
        sessionStorage.removeItem(currentSetCommonQuestionOnQuestionnaireState);
        sessionStorage.removeItem(currentCommonQuestionOfCategoryNameValue);
        sessionStorage.removeItem(currentQuestionListTable);
        sessionStorage.removeItem(currentQuestionListItsControlsDisabledState);
        sessionStorage.removeItem(currentUserList);
        sessionStorage.removeItem(currentUserListShowState);
        sessionStorage.removeItem(currentUserAnswer);
        sessionStorage.removeItem(currentUserAnswerShowState);
        sessionStorage.removeItem(currentUserListPager);
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
        let strUserListShowState = sessionStorage.getItem(currentUserListShowState);
        let strUserListPagerHtml = sessionStorage.getItem(currentUserListPager);
        if (strUserListHtml && currentActiveTab === "#question-info") {
            $(divQuestionListContainer).empty();

            if (strUserListShowState === showState) {
                strUserListHtml === emptyMessageOfUserListOrStatistics
                    ? $(btnExportAndDownloadDataToCSVContainer).hide()
                    : $(btnExportAndDownloadDataToCSVContainer).show();
                $(divUserListContainer).show();
                $(divUserListContainer).html(strUserListHtml);
                $(divUserListPagerContainer).show();
                $(divUserListPagerContainer).html(strUserListPagerHtml);
            }
            else {
                $(btnExportAndDownloadDataToCSVContainer).hide();
                $(divUserListContainer).empty();
                $(divUserListContainer).hide();
                $(divUserListPagerContainer).empty();
                $(divUserListPagerContainer).hide();
            }
        }
        else if (strUserListHtml) {
            if (strUserListShowState === showState) {
                strUserListHtml === emptyMessageOfUserListOrStatistics
                    ? $(btnExportAndDownloadDataToCSVContainer).hide()
                    : $(btnExportAndDownloadDataToCSVContainer).show();
                $(divUserListContainer).show();
                $(divUserListContainer).html(strUserListHtml);
                $(divUserListPagerContainer).show();
                $(divUserListPagerContainer).html(strUserListPagerHtml);
            }
            else {
                $(btnExportAndDownloadDataToCSVContainer).hide();
                $(divUserListContainer).empty();
                $(divUserListContainer).hide();
                $(divUserListPagerContainer).empty();
                $(divUserListPagerContainer).hide();
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
            $(btnExportAndDownloadDataToCSVContainer).hide();
            $(divUserListContainer).show();
            $(divUserListContainer).html(emptyMessageOfUserListOrStatistics);
            $(divUserListPagerContainer).empty();
            $(divUserListPagerContainer).hide();
            $(divStatisticsContainer).html(emptyMessageOfUserListOrStatistics);
        }
        GetQuestionList(strQuestionnaireID);

        $("#ulQuestionnaireDetailTabs li a[data-bs-toggle='tab']").on("show.bs.tab", function () {
            sessionStorage.setItem(activeTab, $(this).attr("href"));
        });
        let strActiveTab = sessionStorage.getItem(activeTab);
        if (strActiveTab) 
            $("#ulQuestionnaireDetailTabs a[href='" + strActiveTab + "']").tab("show");
        if (strActiveTab === "#question") {
            let strQuestionListHtml = sessionStorage.getItem(currentQuestionListTable);
            if (strQuestionListHtml)
                $(divQuestionListContainer).html(strQuestionListHtml);
        }

        let strCurrentSetCommonQuestionOnQuestionnaireState =
            sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
        $(selectCategoryList + " option").each(function () {
            if ($(this).text() === commonQuestionOfCategoryName) 
                sessionStorage.setItem(currentCommonQuestionOfCategoryNameValue, $(this).val());
        });
        let strCurrentCommonQuestionOfCategoryNameValue =
            sessionStorage.getItem(currentCommonQuestionOfCategoryNameValue);
        if (strCurrentSetCommonQuestionOnQuestionnaireState === setState)
            $(selectCategoryList +
                " option[value='" +
                strCurrentCommonQuestionOfCategoryNameValue +
                "']").show();
        else
            $(selectCategoryList +
                " option[value='" +
                strCurrentCommonQuestionOfCategoryNameValue +
                "']").hide();

        $(selectCategoryList).click(function (e) {
            e.preventDefault();

            let strSelectedCategoryText = $(this).find(":selected").text();
            let isSetCustomizedOrCommonQuestionOfCategoryName =
                (strSelectedCategoryText === customizedQuestionOfCategoryName
                || strSelectedCategoryText === commonQuestionOfCategoryName);
            let isUpdateMode = window.location.search.indexOf("?ID=") !== -1;
            let strCurrentSetCommonQuestionOnQuestionnaireState =
                sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
            let strCurrentCommonQuestionOfCategoryNameValue =
                sessionStorage.getItem(currentCommonQuestionOfCategoryNameValue);

            if (strCurrentSetCommonQuestionOnQuestionnaireState === setState) {
                if (strSelectedCategoryText === customizedQuestionOfCategoryName) {
                    let isSetCustomizedQuestion =
                        confirm("選擇常用問題後，\n再次選擇自訂問題，\n會將先前的常用問題全部移除，\n請問仍要繼續嗎？");
                    if (isSetCustomizedQuestion) {
                        $(selectCategoryList +
                            " option[value='" +
                            strCurrentCommonQuestionOfCategoryNameValue +
                            "']").hide();
                        $(selectCategoryList + " option").filter(function () {
                            return $(this).text() == customizedQuestionOfCategoryName;
                        }).prop('selected', true);
                        SetElementCurrentStateSession(
                            currentSetCommonQuestionOnQuestionnaireState,
                            notSetState
                        );
                        DeleteSetQuestionListOfCommonQuestionOnQuestionnaire();
                    }
                    else {
                        $(selectCategoryList + " option").filter(function () {
                            return $(this).text() == commonQuestionOfCategoryName;
                        }).prop('selected', true);
                    }
                }
                else if (!isSetCustomizedOrCommonQuestionOfCategoryName) {
                    let isSetOtherCommonQuestion =
                        confirm("選擇常用問題後，\n再選擇其他常用問題，\n會將先前的常用問題全部移除，\n請問仍要繼續嗎？");
                    if (isSetOtherCommonQuestion) {
                        let strSelectedCategoryID = $(this).val();
                        SetQuestionListOfCommonQuestionOnQuestionnaire(strSelectedCategoryID);
                    }
                }
            }
            else {
                if (isUpdateMode && !isSetCustomizedOrCommonQuestionOfCategoryName) {
                    let isSetAnyTemplateOfCommonQuestion =
                        confirm("如果選擇常用問題，\n現在的自訂問題會全部移除，\n請問仍要繼續嗎？");

                    if (isSetAnyTemplateOfCommonQuestion) {
                        let strSelectedCategoryID = $(this).val();
                        SetQuestionListOfCommonQuestionOnQuestionnaire(strSelectedCategoryID);
                    }
                }
                else if (isSetCustomizedOrCommonQuestionOfCategoryName)
                    return;
                else {
                    let strSelectedCategoryID = $(this).val();
                    SetQuestionListOfCommonQuestionOnQuestionnaire(strSelectedCategoryID);
                }
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
            if (strCurrentSetCommonQuestionOnQuestionnaireState === setState) {
                let isToModifyCommonQuestion =
                    confirm("如果對常用問題進行任何增刪修，\n其將成為自訂問題，\n請問仍要繼續嗎？");
                if (!isToModifyCommonQuestion)
                    return;
                else {
                    $(selectCategoryList + 
                        " option[value='" + 
                        strCurrentCommonQuestionOfCategoryNameValue +
                        "']").hide();
                    $(selectCategoryList + " option").filter(function () {
                        return $(this).text() == customizedQuestionOfCategoryName;
                    }).prop('selected', true);
                    SetElementCurrentStateSession(
                        currentSetCommonQuestionOnQuestionnaireState,
                        notSetState
                    );

                    objQuestion.questionCategory = customizedQuestionOfCategoryName;
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
            $(divQuestionListContainer + " table tbody tr td input[type='checkbox']:checked")
                .each(function () {
                    arrCheckedQuestionID.push($(this).attr("id"));
                });
            if (arrCheckedQuestionID.length === 0) {
                alert("請選擇要刪除的問題。");
                return;
            }

            let strCurrentSetCommonQuestionOnQuestionnaireState =
                sessionStorage.getItem(currentSetCommonQuestionOnQuestionnaireState);
            if (strCurrentSetCommonQuestionOnQuestionnaireState === setState) {
                let isToModifyCommonQuestion = 
                    confirm("如果對常用問題進行任何增刪修，\n其將成為自訂問題，\n請問仍要繼續嗎？");
                if (!isToModifyCommonQuestion)
                    return;
                else {
                    $(selectCategoryList + 
                        " option[value='" + 
                        strCurrentCommonQuestionOfCategoryNameValue +
                        "']").hide();
                    $(selectCategoryList + " option").filter(function () {
                        return $(this).text() == customizedQuestionOfCategoryName;
                    }).prop('selected', true);
                    SetElementCurrentStateSession(
                        currentSetCommonQuestionOnQuestionnaireState,
                        notSetState
                    );
                }
            }

            DeleteQuestionList(arrCheckedQuestionID.join());
        });

        $(document).on("click", divQuestionListContainer + " input[id!=''][type='checkbox']", function () {
            let strDisabledState = sessionStorage.getItem(currentQuestionListItsControlsDisabledState);
            if (strDisabledState === enabledState) {
                $(this).prop("disabled", true);
                alert("此問卷已有使用者的回答，所以勾選無效。");
                return false;
            }
            else 
                $(this).removeProp("disabled");
        });
        $(document).on("click", "a[id*=aLinkEditQuestion]", function (e) {
            e.preventDefault();

            let strDisabledState = sessionStorage.getItem(currentQuestionListItsControlsDisabledState);
            if (strDisabledState === enabledState) {
                $(this).addClass("disabledClick");
                alert("此問卷已有使用者的回答，所以不能修改。");
                return;
            }
            else
                $(this).removeClass("disabledClick");

            if (window.location.search.indexOf("?ID=") === -1) {
                $(this).addClass("disabledClick");
                alert("請先新增後，再編輯。");
                return;
            }
            else
                $(this).removeClass("disabledClick");

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

            $(btnExportAndDownloadDataToCSVContainer).show();
            let strUserListHtml = sessionStorage.getItem(currentUserList);
            let strUserListPagerHtml = sessionStorage.getItem(currentUserListPager);
            $(divUserListContainer).show();
            $(divUserListContainer).html(strUserListHtml);
            $(divUserListPagerContainer).show();
            $(divUserListPagerContainer).html(strUserListPagerHtml);

            SetContainerShowStateSession(currentUserAnswerShowState, hideState);
            SetContainerShowStateSession(currentUserListShowState, showState);
        });
    }
});
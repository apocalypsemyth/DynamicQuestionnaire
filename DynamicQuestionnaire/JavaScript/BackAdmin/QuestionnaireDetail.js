$(document).ready(function () {
    if (window.location.href.indexOf("QuestionnaireDetail.aspx") === -1) {
        if (sessionStorage.getItem("activeTab") != null)
            sessionStorage.removeItem("activeTab");

        if (sessionStorage.getItem("currentQuestionListTable") != null)
            sessionStorage.removeItem("currentQuestionListTable");
    }
    else {
        let strHtml = sessionStorage.getItem("currentQuestionListTable");
        if (strHtml) {
            $("#divGvQuestionListContainer").empty();
            $("#divGvQuestionListContainer").html(strHtml);
        }

        let currentQueryString = window.location.search;
        let isExistQueryString = currentQueryString.indexOf("?ID=") !== -1;
        let strQuestionnaireID = isExistQueryString ? currentQueryString.split("?ID=")[1] : "";
        if (isExistQueryString) 
            GetQuestionnaire(strQuestionnaireID);
        GetQuestionList(strQuestionnaireID);

        $("#ulQuestionnaireDetailTabs li a[data-bs-toggle='tab']").on("show.bs.tab", function () {
            sessionStorage.setItem("activeTab", $(this).attr("href"));
        });
        let activeTab = sessionStorage.getItem("activeTab");
        if (activeTab) {
            $("#ulQuestionnaireDetailTabs a[href='" + activeTab + "']").tab("show");
        }

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
            $("#divGvQuestionListContainer table tbody tr td input[type='checkbox']:checked").each(function () {
                arrCheckedQuestionID.push($(this).attr("id"));
            })

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

            $("#divGvQuestionListContainer table tbody tr td a.currentEditQuestion[id*=aLinkEditQuestion]").removeClass("currentEditQuestion");
            $(this).toggleClass("currentEditQuestion");

            let aLinkHref = $(this).attr("href");
            let strQuestionID = aLinkHref.split("?QuestionID=")[1];
            ShowToUpdateQuestion(strQuestionID);
        });
    }
});
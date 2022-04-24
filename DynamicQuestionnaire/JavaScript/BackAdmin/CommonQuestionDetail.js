$(document).ready(function () {
    if (window.location.href.indexOf("CommonQuestionDetail.aspx") === -1) {
        if (sessionStorage.getItem("currentQuestionListOfCommonQuestionTable") != null)
            sessionStorage.removeItem("currentQuestionListOfCommonQuestionTable");
    }
    else {
        let strHtml = sessionStorage.getItem("currentQuestionListOfCommonQuestionTable");
        if (strHtml) {
            $("#divQuestionListOfCommonQuestionContainer").html(strHtml);
        }

        let currentQueryString = window.location.search;
        let isExistQueryString = currentQueryString.indexOf("?ID=") !== -1;
        let strCommonQuestionID = isExistQueryString ? currentQueryString.split("?ID=")[1] : "";
        if (isExistQueryString) {
            GetCommonQuestion(strCommonQuestionID);
            GetQuestionListOfCommonQuestion(strCommonQuestionID);
        }
        else {
            $("#divQuestionListOfCommonQuestionContainer").html("<p>目前尚未有資料</p>");
        }

        $("button[id=btnAddQuestionOfCommonQuestion]").click(function (e) {
            e.preventDefault();

            let objCommonQuestion = GetCommonQuestionInputs();
            let isValidCommonQuestion = CheckCommonQuestionInputs(objCommonQuestion);
            if (typeof isValidCommonQuestion === "string") {
                alert(isValidCommonQuestion);
                return;
            }

            if (isExistQueryString) {
                UpdateCommonQuestion(objCommonQuestion);

                let objQuestionOfCommonQuestion = GetQuestionOfCommonQuestionInputs();
                let isValidQuestionOfCommonQuestion =
                    CheckQuestionOfCommonQuestionInputs(objQuestionOfCommonQuestion);
                if (typeof isValidQuestionOfCommonQuestion === "string") {
                    alert(isValidQuestionOfCommonQuestion);
                    return;
                }
                
                CreateQuestionOfCommonQuestion(objQuestionOfCommonQuestion);
            }
            else
                CreateCommonQuestion(objCommonQuestion);
        });
        $("button[id=btnDeleteQuestionOfCommonQuestion]").click(function (e) {
            e.preventDefault();

            let arrCheckedQuestionIDOfCommonQuestion = [];
            $("#divQuestionListOfCommonQuestionContainer table tbody tr td input[type='checkbox']:checked")
                .each(function () {
                    arrCheckedQuestionIDOfCommonQuestion.push($(this).attr("id"));
                });

            DeleteQuestionListOfCommonQuestion(arrCheckedQuestionIDOfCommonQuestion.join());
        });

        $(document).on("click", "a.currentEditQuestionOfCommonQuestion[id*=aLinkEditQuestionOfCommonQuestion]", function (e) {
            e.preventDefault();

            if (window.location.search.indexOf("?ID=") === -1) {
                alert("請先新增後，再編輯");
                return;
            }

            $(this).toggleClass("currentEditQuestionOfCommonQuestion");

            let objQuestionOfCommonQuestion = GetQuestionOfCommonQuestionInputs();
            let isValidQuestionOfCommonQuestion =
                CheckQuestionOfCommonQuestionInputs(objQuestionOfCommonQuestion);
            if (typeof isValidQuestionOfCommonQuestion === "string") {
                alert(isValidQuestionOfCommonQuestion);
                return;
            }

            let aLinkHref = $(this).attr("href");
            let strQuestionIDOfCommonQuestion = aLinkHref.split('?QuestionIDOfCommonQuestion=')[1];
            objQuestionOfCommonQuestion.clickedQuestionIDOfCommonQuestion = strQuestionIDOfCommonQuestion;
            UpdateQuestionOfCommonQuestion(objQuestionOfCommonQuestion);
        });
        $(document).on("click", "a:not(.currentEditQuestionOfCommonQuestion)[id*=aLinkEditQuestionOfCommonQuestion]",
            function (e) {
            e.preventDefault();

            if (window.location.search.indexOf("?ID=") === -1) {
                alert("請先新增後，再編輯");
                return;
            }

            $("#divQuestionListOfCommonQuestionContainer table tbody tr td a.currentEditQuestionOfCommonQuestion[id*=aLinkEditQuestionOfCommonQuestion]").removeClass("currentEditQuestionOfCommonQuestion");
            $(this).toggleClass("currentEditQuestionOfCommonQuestion");

            let aLinkHref = $(this).attr("href");
            let strQuestionIDOfCommonQuestion = aLinkHref.split("?QuestionIDOfCommonQuestion=")[1];
            ShowToUpdateQuestionOfCommonQuestion(strQuestionIDOfCommonQuestion);
        });
    }
})
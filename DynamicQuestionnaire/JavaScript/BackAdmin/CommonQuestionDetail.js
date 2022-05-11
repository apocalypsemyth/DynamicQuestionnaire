$(document).ready(function () {
    if (window.location.href.indexOf("CommonQuestionDetail.aspx") === -1) {
        sessionStorage.removeItem(currentQuestionListOfCommonQuestionTable);
    }
    else {
        let strHtml = sessionStorage.getItem(currentQuestionListOfCommonQuestionTable);
        if (strHtml) 
            $(divQuestionListOfCommonQuestionContainer).html(strHtml);

        let currentQueryString = window.location.search;
        let isExistQueryString = currentQueryString.indexOf("?ID=") !== -1;
        let strCommonQuestionID = isExistQueryString ? currentQueryString.split("?ID=")[1] : "";
        if (isExistQueryString)
            GetCommonQuestion(strCommonQuestionID);
        else {
            $(btnDeleteQuestionOfCommonQuestion).hide();
            $(divQuestionListOfCommonQuestionContainer).html(emptyMessageOfQuestionList);
        }
        GetQuestionListOfCommonQuestion(strCommonQuestionID);

        $(btnAddQuestionOfCommonQuestion).click(function (e) {
            e.preventDefault();

            let objCommonQuestion = GetCommonQuestionInputs();
            let isValidCommonQuestion = CheckCommonQuestionInputs(objCommonQuestion);
            if (typeof isValidCommonQuestion === "string") {
                alert(isValidCommonQuestion);
                return;
            }

            let objQuestionOfCommonQuestion = GetQuestionOfCommonQuestionInputs();
            let isValidQuestionOfCommonQuestion =
                CheckQuestionOfCommonQuestionInputs(objQuestionOfCommonQuestion);
            if (typeof isValidQuestionOfCommonQuestion === "string") {
                alert(isValidQuestionOfCommonQuestion);
                return;
            }

            if (window.location.search.indexOf("?ID=") !== -1) {
                let btnHref = $(this).attr("href");

                if (btnHref) {
                    let strQuestionIDOfCommonQuestion = $(this).attr("href");
                    objQuestionOfCommonQuestion.clickedQuestionIDOfCommonQuestion = strQuestionIDOfCommonQuestion;
                    UpdateQuestionOfCommonQuestion(objQuestionOfCommonQuestion);
                }
                else {
                    UpdateCommonQuestion(objCommonQuestion);
                    CreateQuestionOfCommonQuestion(objQuestionOfCommonQuestion);
                }
            }
            else
                CreateCommonQuestion(objCommonQuestion);
        });
        $(btnDeleteQuestionOfCommonQuestion).click(function (e) {
            e.preventDefault();

            let arrCheckedQuestionIDOfCommonQuestion = [];
            $(divQuestionListOfCommonQuestionContainer + " table tbody tr td input[type='checkbox']:checked")
                .each(function () {
                    arrCheckedQuestionIDOfCommonQuestion.push($(this).attr("id"));
                });
            if (arrCheckedQuestionIDOfCommonQuestion.length === 0) {
                alert("請選擇要刪除的問題。");
                return;
            }

            DeleteQuestionListOfCommonQuestion(arrCheckedQuestionIDOfCommonQuestion.join());
        });

        $(document).on("click", "a[id*=aLinkEditQuestionOfCommonQuestion]", function (e) {
            e.preventDefault();

            if (window.location.search.indexOf("?ID=") === -1) {
                alert("請先新增後，再編輯");
                return;
            }

            let aLinkHref = $(this).attr("href");
            let strQuestionIDOfCommonQuestion = aLinkHref.split("?QuestionIDOfCommonQuestion=")[1];
            $(btnAddQuestionOfCommonQuestion).attr("href", strQuestionIDOfCommonQuestion);
            ShowToUpdateQuestionOfCommonQuestion(strQuestionIDOfCommonQuestion);
        });
    }
})
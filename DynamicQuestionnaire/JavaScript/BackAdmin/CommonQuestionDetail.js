$(document).ready(function () {
    if (window.location.href.indexOf("CommonQuestionDetail.aspx") === -1) {
        if (sessionStorage.getItem("currentQuestionListOfCommonQuestionTable") != null)
            sessionStorage.removeItem("currentQuestionListOfCommonQuestionTable");
    }
    else {
        let strHtml = sessionStorage.getItem("currentQuestionListOfCommonQuestionTable");
        if (strHtml) {
            $("#divQuestionListOfCommonQuestionContainer").empty();
            $("#divQuestionListOfCommonQuestionContainer").html(strHtml);
        }

        let currentQueryString = window.location.search;
        let isExistQueryString = currentQueryString.indexOf("?ID=") !== -1;
        let strCommonQuestionID = isExistQueryString ? currentQueryString.split("?ID=")[1] : "";
        //if (isExistQueryString) {
        //    GetQuestionnaire(strCommonQuestionID);

        //    if (!strUserListHtml)
        //        GetUserList(strCommonQuestionID);
        //    else if (strUserListHtml && strUserListHtml === "true")
        //        GetUserList(strCommonQuestionID);
        //}
        GetQuestionListOfCommonQuestion(strCommonQuestionID);

        $("button[id=btnAddQuestionOfCommonQuestion]").click(function (e) {
            e.preventDefault();

            let objCommonQuestion = GetCommonQuestionInputs();
            let isValidCommonQuestion = CheckCommonQuestionInputs(objCommonQuestion);
            if (typeof isValidCommonQuestion === "string") {
                alert(isValidCommonQuestion);
                return;
            }

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
    }
})
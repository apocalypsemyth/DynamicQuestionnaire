$(document).ready(function () {
    $("a[id*=aLinkCheckingQuestionnaireDetail]").click(function () {
        ResetUserInputsIsInvalidClass();
        ResetUserInputs();

        let objUser = GetUserInputs();
        let isValidUserInputs = CheckUserInputs(objUser);
        if (!isValidUserInputs) {
            alert("填寫的資訊有錯，請您再檢查。");
            return false;
        }

        let isValidRequiredQuestionInputs = CheckRequiredQuestionInputs();
        if (typeof isValidRequiredQuestionInputs === "string") {
            alert(isValidRequiredQuestionInputs);
            return false;
        }

        let isValidAtLeastOneQuestionInputs = CheckAtLeastOneQuestionInputs();
        if (typeof isValidAtLeastOneQuestionInputs === "string") {
            alert(isValidAtLeastOneQuestionInputs);
            return false;
        }

        let currentQuestionnaireID = window.location.search.split("?ID=")[1];
        objUser.questionnaireID = currentQuestionnaireID;
        $.ajax({
            async: false,
            url: "/API/QuestionnaireDetailDataHandler.ashx?Action=CREATE_USER",
            method: "POST",
            data: objUser,
            error: function (msg) {
                console.log(msg);
                alert("通訊失敗，請聯絡管理員。");
            }
        });

        $.ajax({
            async: false,
            url: "/API/QuestionnaireDetailDataHandler.ashx?Action=CREATE_USERANSWER",
            method: "POST",
            data: { "userAnswer": isValidAtLeastOneQuestionInputs.join(";") },
            error: function (msg) {
                console.log(msg);
                alert("通訊失敗，請聯絡管理員。");
            }
        });
    });
});
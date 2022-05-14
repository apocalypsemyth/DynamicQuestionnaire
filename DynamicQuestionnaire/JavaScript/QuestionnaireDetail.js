$(document).ready(function () {
    if (window.location.href.indexOf("QuestionnaireDetail.aspx") === -1) {
        ResetCheckingOrNotQuestionnaireDetailSession();
    }
    else {
        let queryString = window.location.search;
        let isExistQueryString = queryString.indexOf("?ID=") !== -1;
        let isCheckingQuestionnaireDetail =
            window.location.href.indexOf("CheckingQuestionnaireDetail.aspx") !== -1
        let strQuestionnaireID =
            isExistQueryString
                ? !isCheckingQuestionnaireDetail
                    ? queryString.split("?ID=")[1]
                    : NULL
                : FAILED;
        ResetQuestionnaireDetailInputs(strQuestionnaireID);

        $("a[id*=aLinkCheckingQuestionnaireDetail]").click(function () {
            ResetUserInputsItsIsInvalidClass();
            ResetUserInputsItsValidMessage();

            let objUser = GetUserInputs();
            let isValidUserInputs = CheckUserInputs(objUser);
            if (!isValidUserInputs) {
                alert("填寫的個資有錯，請再檢查。");
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
                    alert(errorMessageOfAjax);
                }
            });

            $.ajax({
                async: false,
                url: "/API/QuestionnaireDetailDataHandler.ashx?Action=CREATE_USERANSWER",
                method: "POST",
                data: { "userAnswer": isValidAtLeastOneQuestionInputs.join(";") },
                error: function (msg) {
                    console.log(msg);
                    alert(errorMessageOfAjax);
                }
            });
        });
    }
});
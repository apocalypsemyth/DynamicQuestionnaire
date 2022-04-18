$(document).ready(function () {
    $("a[id*=aLinkCheckingQuestionnaireDetail]").click(function () {
        const SINGLE_SELECT = "單選方塊";
        const MULTIPLE_SELECT = "複選方塊";
        const TEXT = "文字";

        ResetUserInputsIsInvalidClass();
        ResetUserInputs();

        let objUser = GetUserInputs();
        let isValidUserInputs = CheckUserInputs(objUser);
        if (!isValidUserInputs) 
            return false;
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

        let arrResult = [];

        if ($("input:radio[id*=rdoQuestionAnswer]").length) {
            arrResult.push(
                GetUserAnswerInputs(
                    "input:radio[id*=rdoQuestionAnswer]:checked"
                    , SINGLE_SELECT)
            );
        }

        if ($("input:checkbox[id*=ckbQuestionAnswer]").length) {
            arrResult.push(
                GetUserAnswerInputs(
                    "input:checkbox[id*=ckbQuestionAnswer]:checked"
                    , MULTIPLE_SELECT)
            );
        }

        if ($("input[id*=txtQuestionAnswer]").length) {
            arrResult.push(
                GetUserAnswerInputs(
                    "input[id*=txtQuestionAnswer]"
                    , TEXT)
            );
        }

        let arrUserAnswer = $.map(arrResult, function (n) {
            return n;
        })

        if (!arrUserAnswer.length) {
            alert("請至少作答一個問題。");
            return false;
        }
        console.log(arrUserAnswer.join(";"));
        $.ajax({
            async: false,
            url: "/API/QuestionnaireDetailDataHandler.ashx?Action=CREATE_USERANSWER",
            method: "POST",
            data: { "userAnswer": arrUserAnswer.join(";") },
            success: function (arrUserAnswerModel) {
                console.log(arrUserAnswerModel);
            },
            error: function (msg) {
                console.log(msg);
                alert("通訊失敗，請聯絡管理員。");
            }
        });
    });
});
$(document).ready(function () {
    const SINGLE_SELECT = "單選方塊";
    const MULTIPLE_SELECT = "複選方塊";
    const TEXT = "文字";

    $("a[id*=aLinkCheckingQuestionnaireDetail]").click(function () {
        let isValidUserInputs = CheckUserInputs();
        if (!isValidUserInputs) {
            alert("Error");
            return false;
        } 

        let arrResult = [];

        if ($("input:radio[id*=rdoQuestionAnswer]").length) {
            arrResult.push(
                GetUserQuestionAnswerInputs(
                    "input:radio[id*=rdoQuestionAnswer]:checked"
                    , SINGLE_SELECT)
            );
        }

        if ($("input:checkbox[id*=ckbQuestionAnswer]").length) {
            arrResult.push(
                GetUserQuestionAnswerInputs(
                    "input:checkbox[id*=ckbQuestionAnswer]:checked"
                    , MULTIPLE_SELECT)
            );
        }

        if ($("input[id*=txtQuestionAnswer]").length) {
            arrResult.push(
                GetUserQuestionAnswerInputs(
                    "input[id*=txtQuestionAnswer]"
                    , TEXT)
            );
        }

        let arrUserQuestion = $.map(arrResult, function (n) {
            return n;
        })

        if (arrUserQuestion.length) 
            CreateUserQuestion(arrUserQuestion.join(";"));
    });
});
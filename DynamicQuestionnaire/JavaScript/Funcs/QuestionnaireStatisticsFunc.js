function CreatePieChart(strSelector, strQuestionName, arrQuestionAnswer, arrEachUserAnswerNum) {
    function setTranslation(p, slice) {
        p.sliced = slice;
        if (p.sliced) {
            p.graphic.animate(p.slicedTranslation);
        }
        else {
            p.graphic.animate({
                translateX: 0,
                translateY: 0
            });
        }
    }

    var options = {
        chart: {
            renderTo: strSelector,
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: null
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b> ({point.y})'
        },
        plotOptions: {
            pie: {
                point: {
                    events: {
                        mouseOut: function () {
                            setTranslation(this, false);
                        },
                        mouseOver: function () {
                            setTranslation(this, true);
                        }
                    }
                },
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f}% ({point.y})',
                    style: {
                        fontWeight: 'bold',
                        fontSize: '1rem',
                    }
                }
            }
        },
        series: [],
        credits: {
            enabled: false
        }
    }

    var seriesOptions = {
        name: strQuestionName,
        data: [],
        colorByPoint: true,
    }

    for (i = 0; i < arrQuestionAnswer.length; i++) {
        var dataOptions = {
            name: '',
            y: ''
        }
        dataOptions.name = arrQuestionAnswer[i];
        dataOptions.y = arrEachUserAnswerNum[i] == null ? 0 : arrEachUserAnswerNum[i];
        seriesOptions.data.push(dataOptions);
    }

    options.series.push(seriesOptions);
    var chart = new Highcharts.Chart(options);
}
function CreateQuestionnaireStatistics(objArrQuestionModel, objArrUserAnswerModel) {
    $(divQuestionnaireStatisticsContainer).append(
        `
            <div class="row align-items-center justify-content-center">
                <div class="col-md-10">
                    <div id="divQuestionnaireStatisticsInnerContainer"
                         class="row align-items-center justify-content-center gy-3 gy-md-5"
                    >
                    </div>
                </div>
            </div>
        `
    );

    let i = 1;
    for (let question of objArrQuestionModel) {
        let questionID = question.QuestionID;
        let questionName = question.QuestionName;
        let questionRequired = question.QuestionRequired;
        let questionTyping = question.QuestionTyping;
        let arrQuestionAnswer = question.QuestionAnswer.split(";");

        let arrQuestionItsUserAnswer = objArrUserAnswerModel
            .filter(item => item.QuestionID === questionID);

        let arrUserAnswerNum = [];
        if (arrQuestionItsUserAnswer.length)
            arrUserAnswerNum = arrQuestionItsUserAnswer.map(item2 => item2.AnswerNum - 1);
        else
            arrUserAnswerNum.push(-1);

        let arrEachUserAnswerNum = [];
        if (arrUserAnswerNum.indexOf(-1) === -1) {
            arrEachUserAnswerNum = arrUserAnswerNum.reduce((acc, val) => {
                acc[val] = acc[val] == null ? 1 : acc[val] += 1;
                return acc;
            }, []);
        }
        else
            arrEachUserAnswerNum = new Array(arrQuestionAnswer.length).fill(null);

        let resultQuestionName = `${i}. ${questionName} ${questionRequired ? "(必填)" : ""}`;

        if (!arrQuestionItsUserAnswer.length) {
            $("#divQuestionnaireStatisticsInnerContainer").append(
                `
                    <div class="col-12">
                        <div class="d-flex flex-column">
                            <h2>
                                ${resultQuestionName}
                            </h2>
                            <p>
                                目前尚未有使用者的回答
                            </p>
                        </div>
                    </div>
                `
            );
        }
        else if (questionTyping === "文字") {
            $("#divQuestionnaireStatisticsInnerContainer").append(
                `
                    <div class="col-12">
                        <div class="d-flex flex-column">
                            <h2>
                                ${resultQuestionName}
                            </h2>
                            <p>
                                -
                            </p>
                        </div>
                    </div>
                `
            );
        }
        else {
            let dynamicQuestionnaireStatistics = `divQuestionnaireStatistics_${questionID}`;

            $("#divQuestionnaireStatisticsInnerContainer").append(
                `
                    <div class="col-12">
                        <div class="d-flex flex-column">
                            <h2>
                                ${resultQuestionName}
                            </h2>
                            <div id="${dynamicQuestionnaireStatistics}"></div>
                        </div>
                    </div>
                `
            );

            CreatePieChart(
                dynamicQuestionnaireStatistics,
                questionName,
                arrQuestionAnswer,
                arrEachUserAnswerNum
            );
        }

        i++;
    }
}
function GetQuestionnaireStatistics(strQuestionnaireID) {
    $.ajax({
        url: "/API/QuestionnaireStatisticsDataHandler.ashx?Action=GET_QUESTIONNAIRE_STATISTICS",
        method: "POST",
        data: { "questionnaireID": strQuestionnaireID },
        success: function (strOrObjArrStatistics) {
            if (strOrObjArrStatistics === FAILED)
                alert("發生錯誤，請再嘗試");
            else {
                let [objArrQuestionModel, objArrUserAnswerModel] = strOrObjArrStatistics;
                
                CreateQuestionnaireStatistics(objArrQuestionModel, objArrUserAnswerModel);
            }
        },
        error: function (msg) {
            console.log(msg);
            alert("通訊失敗，請聯絡管理員。");
        }
    });
}
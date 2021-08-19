<template>
    <div>
        <b-card no-body class="mb-2">
            <b-card-body>
                <b-card-text>
                    <b-row>
                        <b-col cols="12">
                            <ValidationObserver ref="setLocaleQuestion">
                                <b-form>
                                    <b-row>
                                        <b-col cols="12" class="mb-2">
                                            <b-button
                                                class="mr-2"
                                                v-b-modal.correct-question
                                                variant="outline-warning"
                                                v-if="this.mode"
                                                >تصحيح سؤال</b-button
                                            >
                                            <b-button
                                                v-b-modal.correct-answer
                                                variant="outline-warning"
                                                v-if="this.mode"
                                                >تصحيح جواب</b-button
                                            >
                                        </b-col>
                                        <b-col cols="12">
                                            <label for="question-text"
                                                >نص السؤال</label
                                            >
                                            <EKInputTextarea
                                                size="sm"
                                                class="mb-1"
                                                :rules="[
                                                    {
                                                        type: 'required',
                                                        message:
                                                            'نص السؤال مطلوب'
                                                    }
                                                ]"
                                                v-model="questonsDto.title"
                                                name="title"
                                            />
                                        </b-col>
                                        <b-col cols="12" v-if="!typeTextAns">
                                            <label for="question-text"
                                                >نص الجواب</label
                                            >
                                             <EKInputTextarea
                                                v-if="questonsDto.answers == null || questonsDto.answers.length == 0"
                                                size="sm"
                                                class="mb-1"
                                                :rules="[
                                                    {
                                                        type: 'required',
                                                        message:
                                                            'نص الجواب مطلوب'
                                                    }
                                                ]"
                                                v-model="answer.title"
                                                name="title"
                                            />
                                            <EKInputTextarea
                                                v-else
                                                size="sm"
                                                class="mb-1"
                                                :rules="[
                                                    {
                                                        type: 'required',
                                                        message:
                                                            'نص الجواب مطلوب'
                                                    }
                                                ]"
                                                v-model="questonsDto.answers[0].title"
                                                name="title"
                                            />
                                        </b-col>
                                        <b-col cols="12" v-else>
                                            <b-list-group-item
                                                v-b-toggle.answer-collapse
                                                class="shadow mb-1 rounded border-0"
                                            >
                                                الأجوبة
                                            </b-list-group-item>
                                            <b-collapse
                                                id="answer-collapse"
                                                class="mb-1"
                                            >
                                                <b-list-group>
                                                    <b-form-radio-group
                                                        v-model="
                                                            correctAnswearIndex
                                                        "
                                                    >
                                                        <template
                                                            v-for="(answer,
                                                            index) in questonsDto.answers"
                                                        >
                                                            <b-list-group-item
                                                                class="shadow border-0 mb-1 d-flex pr-0 rounded align-items-center"
                                                                v-if="
                                                                    !answer.correctionDate
                                                                "
                                                                :key="index"
                                                                v-b-toggle="
                                                                    'answer-collapse' +
                                                                        index
                                                                "
                                                            >
                                                                <b-form-radio
                                                                    :variant="
                                                                        answer.correctAnswerId
                                                                            ? 'warning'
                                                                            : ''
                                                                    "
                                                                    :value="
                                                                        index
                                                                    "
                                                                ></b-form-radio>
                                                                <div
                                                                    class="w-100"
                                                                >
                                                                    <p
                                                                        class="m-0"
                                                                        :class="{
                                                                            'text-warning':
                                                                                answer.correctAnswerId
                                                                        }"
                                                                    >
                                                                        {{
                                                                            answer.title
                                                                        }}
                                                                    </p>
                                                                    <b-collapse
                                                                        v-if="
                                                                            answer.correctAnswerId
                                                                        "
                                                                        :id="
                                                                            'answer-collapse' +
                                                                                index
                                                                        "
                                                                        class="mb-1 w-100 mt-1"
                                                                    >
                                                                        <template
                                                                            v-for="(ans,
                                                                            i) in questonsDto.answers"
                                                                        >
                                                                            <b-list-group-item
                                                                                :key="
                                                                                    i
                                                                                "
                                                                                v-if="
                                                                                    ans.id ==
                                                                                        answer.correctAnswerId
                                                                                "
                                                                                class="d-flex"
                                                                            >
                                                                          
                                                                                <unicon v-if="ans.isCorrect" name="check" fill="#28c76f"/>
                                                                                <unicon v-else name="times" fill="#ea5455"/>
                                                                                <p
                                                                                    class="m-0"
                                                                                >
                                                                                    {{
                                                                                        ans.title
                                                                                    }}
                                                                                </p>
                                                                            </b-list-group-item>
                                                                        </template>
                                                                    </b-collapse>
                                                                </div>
                                                                <b-button
                                                                    size="sm"
                                                                    class="ml-auto"
                                                                    variant="link"
                                                                    @click="
                                                                        removeAnswear(
                                                                            index,
                                                                            questonsDto.answers
                                                                        )
                                                                    "
                                                                >
                                                                    <unicon
                                                                        name="trash-alt"
                                                                        fill="#FF6330"
                                                                    ></unicon>
                                                                </b-button>
                                                            </b-list-group-item>
                                                        </template>
                                                    </b-form-radio-group>
                                                    <b-list-group-item
                                                        class="mb-2 d-flex flex-column justify-content-center align-content-center border-0 shadow rounded"
                                                    >
                                                        <label
                                                            for="question-text"
                                                            >إجابة جديدة</label
                                                        >
                                                        <b-form-textarea
                                                            size="sm"
                                                            v-model="newAns"
                                                        ></b-form-textarea>
                                                        <hr class="w-100" />
                                                        <div
                                                            class="d-flex justify-content-between"
                                                        >
                                                            <small
                                                                class="text-danger"
                                                                v-show="
                                                                    questonsDto
                                                                        .answers
                                                                        .length <
                                                                        2
                                                                "
                                                                >يطلب على الأقل
                                                                جوابين</small
                                                            >
                                                            <b-button
                                                                size="sm"
                                                                variant="primary"
                                                                class="ml-auto"
                                                                @click="
                                                                    addAnswear
                                                                "
                                                            >
                                                                <unicon
                                                                    name="plus"
                                                                    fill="#fff"
                                                                ></unicon>
                                                            </b-button>
                                                        </div>
                                                    </b-list-group-item>
                                                </b-list-group>
                                            </b-collapse>
                                        </b-col>
                                        <b-col cols="12">
                                            <EKInputText
                                                label="تلميح السؤال"
                                                placeholder="ادخل التلميح"
                                                v-model="questonsDto.hint"
                                                name="hint"
                                            />
                                        </b-col>
                                        <b-col cols="12">
                                            <EKInputSelect
                                                class="mb-2"
                                                label="التصنيفات"
                                                placeholder="اختر التصنيفات"
                                                v-model="questonsDto.tags"
                                                :options="tagsList"
                                                name="tags"
                                                multiple
                                            />
                                        </b-col>
                                        <b-col cols="12">
                                            <EKInputImage
                                                label="صورة المادة"
                                                mul
                                                title="upload image"
                                                :value="questonsDto.images"
                                                @input="
                                                    questonsDto.images = $event
                                                "
                                            >
                                                <template
                                                    slot="preview"
                                                    slot-scope="{ previewBase }"
                                                >
                                                    <div
                                                        class="d-flex justify-content-between flex-wrap border-top px-1 pt-1"
                                                    >
                                                        <img
                                                            v-for="(base,
                                                            index) in previewBase"
                                                            :src="base"
                                                            style="object-fit: cover;"
                                                            width="200"
                                                            height="200"
                                                            :key="index"
                                                            class="mb-1 border"
                                                        />
                                                    </div>
                                                </template>
                                            </EKInputImage>
                                        </b-col>
                                    </b-row>
                                </b-form>
                            </ValidationObserver>
                        </b-col>
                        <b-col>
                            <ValidationObserver ref="addQuestionLinks">
                                <b-form @submit.prevent="addQuestionLinks">
                                    <b-col
                                        cols="12"
                                        class="border rounded pt-1"
                                    >
                                        <b-row>
                                            <b-col cols="12" lg="4">
                                                <EKInputSelect
                                                    label="المادة"
                                                    placeholder="اختر المادة"
                                                    :options="subjectsList"
                                                    v-model="
                                                        questionFilterDto.subjectID
                                                    "
                                                    name="subjectsList"
                                                    :clearable="true"
                                                />
                                            </b-col>
                                            <b-col
                                                cols="12"
                                                lg="4"
                                                v-if="!typeTextAns"
                                            >
                                                <EKInputSelect
                                                    label="الأسئلة الكتابية"
                                                    placeholder="اختر الأسئلة الكتابية"
                                                    :options="
                                                        filterdInterviewsBySubject
                                                    "
                                                    v-model="selectedInterviews"
                                                    multiple
                                                    :rules="[
                                                        {
                                                            type: 'required',
                                                            message:
                                                                'الرقم مطلوب'
                                                        }
                                                    ]"
                                                    name="interviewsList"
                                                    :clearable="true"
                                                />
                                            </b-col>
                                            <b-col
                                                cols="12"
                                                lg="4"
                                                v-if="typeTextAns"
                                            >
                                                <EKInputSelect
                                                    label="الدورات"
                                                    placeholder="اختر دورة"
                                                    :options="
                                                        filterdCoursesBySubject
                                                    "
                                                    v-model="selectedCourses"
                                                    multiple
                                                    name="courcesList"
                                                    :clearable="true"
                                                />
                                            </b-col>
                                            <b-col
                                                cols="12"
                                                lg="4"
                                                v-if="typeTextAns"
                                            >
                                                <EKInputSelect
                                                    label="البنوك"
                                                    placeholder="اختر البنك"
                                                    :options="
                                                        filterdBanksBySubject
                                                    "
                                                    v-model="selectedBanks"
                                                    multiple
                                                    name="banksList"
                                                    :clearable="true"
                                                />
                                            </b-col>
                                            <b-col cols="12" lg="4">
                                                <EKInputText
                                                    label="الرقم"
                                                    placeholder="اختر الرقم"
                                                    name="select_assasa"
                                                    :rules="[
                                                        {
                                                            type: 'required',
                                                            message:
                                                                'الرقم مطلوب'
                                                        },
                                                        {
                                                            type: 'min_value:1',
                                                            message:
                                                                'الرقم ان يكون اكبر او يساوي ال 1'
                                                        }
                                                    ]"
                                                    type="number"
                                                    v-model="questionOrder"
                                                />
                                            </b-col>
                                            <b-col
                                                class="d-flex align-items-end pb-1"
                                            >
                                                <b-button
                                                    block
                                                    type="submit"
                                                    variant="outline-primary"
                                                    >إضافة</b-button
                                                >
                                            </b-col>
                                            <b-col cols="12" v-if="typeTextAns">
                                                <h5
                                                    class="text-center mb-1 text-warning"
                                                >
                                                    <small
                                                        >يجب تحديد بنك واحد على
                                                        الأقل او دورة
                                                        واحدة</small
                                                    >
                                                </h5>
                                            </b-col>
                                        </b-row>
                                    </b-col>
                                </b-form>
                            </ValidationObserver>
                        </b-col>
                        <b-col cols="12">
                            <EKTable
                                :columns="banksExamsSubjectsHeader"
                                :items="exams"
                                no_select
                            >
                                <template
                                    slot="items.dateCreated"
                                    slot-scope="{ value }"
                                >
                                    {{
                                        new Date(value).toLocaleDateString(
                                            "en-GB"
                                        )
                                    }}
                                </template>
                                <template
                                    slot="items.delete"
                                    slot-scope="{ props }"
                                >
                                    <b-button
                                        size="sm"
                                        class="ml-auto"
                                        variant="link"
                                        @click="
                                            exams.splice(
                                                props.row.originalIndex,
                                                1
                                            )
                                        "
                                    >
                                        <unicon
                                            name="trash-alt"
                                            height="18"
                                            width="18"
                                        ></unicon>
                                    </b-button>
                                </template>
                            </EKTable>
                        </b-col>
                    </b-row>
                </b-card-text>
            </b-card-body>
            <b-card-footer class="d-flex">
                <b-button
                    class="mr-1"
                    variant="primary"
                    @click="setLocaleQuestion"
                    >{{ !this.mode ? "إضافة" : "تعديل" }}</b-button
                >
                <b-button
                    variant="outline-primary"
                    :to="prevRoute.name ? prevRoute.fullPath : '/questions'"
                    >تراجع</b-button
                >
                <b-button
                    @click="onDelete"
                    class="ml-auto"
                    variant="outline-danger"
                    v-if="this.mode"
                    >حذف</b-button
                >
            </b-card-footer>
        </b-card>
        <b-modal
            size="lg"
            id="correct-question"
            @show="getCorrectionQuestion"
            @hidden="resetCorrectQuestion"
            @ok="submitCorrectQuestion"
            ok-title="تم"
            cancel-title="إلغاء"
            title="تصحيح السؤال"
        >
            <ValidationObserver ref="correctQuestionObserver">
                <label>نص السؤال</label>
                <EKInputTextarea
                    size="sm"
                    class="mb-1"
                    :rules="[
                        {
                            type: 'required',
                            message: 'نص السؤال مطلوب'
                        }
                    ]"
                    v-model="localeQuestonsDto.title"
                    name="correctQuestion"
                />
            </ValidationObserver>
        </b-modal>
        <b-modal
            size="lg"
            id="correct-answer"
            @show="getCorrectionQuestion"
            @hidden="resetCorrectQuestion"
            @ok="submitCorrectQuestion"
            ok-title="تم"
            cancel-title="إلغاء"
            title="تصحيح الجواب"
        >
            <ValidationObserver ref="correctAnsObserver">
                <b-row>
                    <b-col cols="12" v-if="!typeTextAns">
                        <label>نص الجواب</label>
                        <EKInputTextarea
                            size="sm"
                            class="mb-1"
                            :rules="[
                                {
                                    type: 'required',
                                    message: 'نص الجواب مطلوب'
                                }
                            ]"
                            v-model="localeQuestonsDto.textAns"
                            name="answear"
                        />
                    </b-col>
                    <b-col cols="12" v-else>
                        <b-list-group-item class="shadow mb-1 rounded border-0">
                            الأجوبة
                        </b-list-group-item>
                        <b-list-group>
                            <b-form-radio-group
                                v-model="localeQuestonsDto.correctAnswearIndex"
                            >
                                <template
                                    v-for="(answer,
                                    index) in localeQuestonsDto.answers"
                                >
                                    <b-list-group-item
                                        class="shadow border-0 mb-1 d-flex pr-0 rounded align-items-center"
                                        v-if="!answer.correctionDate"
                                        :key="index"
                                        :class="
                                            answer.correctAnswerId
                                                ? 'border-warning'
                                                : 'border-0'
                                        "
                                    >
                                        <b-form-radio
                                            :value="index"
                                        ></b-form-radio>
                                        <EKInputTextarea
                                            size="sm"
                                            class="w-100 mt-1 mr-1"
                                            :rules="[
                                                {
                                                    type: 'required',
                                                    message: 'نص الجواب مطلوب'
                                                }
                                            ]"
                                            v-model="answer.title"
                                            name="answear"
                                        />
                                    </b-list-group-item>
                                </template>
                            </b-form-radio-group>
                        </b-list-group>
                    </b-col>
                </b-row>
            </ValidationObserver>
        </b-modal>
    </div>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import { mapState, mapActions, mapGetters } from "vuex";
import EKInputImage from "@Ekcore/components/EK-forms/EK-input-image";
import EKTable from "@Ekcore/components/EK-table";

export default {
    props: {
        id: String,
        ansType: String,
        examId : String,
        subjectId: String
    },
    components: {
        ValidationObserver,
        EKInputSelect,
        EKInputText,
        EKInputTextarea,
        EKTable,
        EKInputImage
    },
    data: () => ({
        localeQuestonsDto: {
            id: "",
            title: "",
            questionType: 0,
            answerType: 0,
            isCorrected: false,
            dateCreated: new Date(),
            answers: [],
            textAns: "",
            correctAnswearIndex: -1
        },
        textAns: "",
        banksExamsSubjectsHeader: [
            {
                label: "رقم السؤال",
                field: "order",
                type: Number
            },
            {
                label: "تابع ل",
                field: "examName"
            },
            {
                label: "تاريخ الإضافة",
                field: "dateCreated"
            },
            {
                label: "إزالة",
                field: "delete"
            }
        ],
        selectedBanks: [],
        selectedCourses: [],
        selectedInterviews: [],
        newAns: "",
        questionOrder: 0,
        correctAnswearIndex: 0,
        answer:{
            id: "",
            title: "",
            isCorrect: false,
            option: "",
            correctionDate: new Date(),
            questionId: "",
            correctAnswerId: ""
        }
    }),
    computed: {
        ...mapState({
            questonsDto: state => state.questions.questonsDto,
            subjectsList: state => state.subjects.subjectsList,
            questionFilterDto: state => state.questions.questionFilterDto,
            exams: state => state.questions.questonsDto.exams,
            prevRoute: state => state.app.prevRoute
        }),
        ...mapGetters([
            "filterdCoursesBySubject",
            "filterdBanksBySubject",
            "filterdInterviewsBySubject",
            "tagsList"
        ])
    },
    setup(props) {
        const mode = props.id != ("" | 0);
        const typeTextAns = +props.ansType;
        
        return { mode, typeTextAns};
    },
    created() {
        if(this.examId != 0){
            this.selectedCourses.unshift(this.examId)
            this.selectedInterviews.unshift(this.examId)
        }
        if(this.subjectId != 0){
            this.questionFilterDto.subjectID = this.subjectId
        }
        
        this.fetchTotalTag();
        this.fetchSubject();
        this.getBasicExams();
        this.fetchInterviews();
        if (this.mode) {
            this.getQuestionDetails(this.id);
        } else {
            this.$store.commit("Reset_Question_Dto");
        }
        
    },
    methods: {
        ...mapActions([
            "getQuestionDetails",
            "fetchSubject",
            "getBasicExams",
            "fetchInterviews",
            "setQuestion",
            "updateQuestion",

            "fetchTotalTag",
            "deleteQuestion",
            "correctQuestion"
        ]),
        submitCorrectQuestion() {
            this.localeQuestonsDto.answers.forEach((ans) => {ans.isCorrect = false})
            this.localeQuestonsDto.answers[this.localeQuestonsDto.correctAnswearIndex].isCorrect = true
            if (!this.typeTextAns) {
                this.correctQuestion({
                    id: this.localeQuestonsDto.id,
                    title: this.localeQuestonsDto.title,
                    isCorrected: true,
                    answers: this.localeQuestonsDto.textAns.trim() != this.questonsDto.answers[this.correctAnswearIndex].title.trim() ? [
                        {
                            correctAnswerId: this.questonsDto.answers[this.correctAnswearIndex].id,
                            title: this.localeQuestonsDto.textAns,
                            isCorrect: true,
                            correctionDate: new Date()
                        },
                        ...this.questonsDto.answers
                    ] : [...this.questonsDto.answers]
                });
            } else {
                this.correctQuestion({
                    id: this.localeQuestonsDto.id,
                    title: this.localeQuestonsDto.title,
                    isCorrected: true,
                    answers: [
                        ...this.localeQuestonsDto.answers
                            .filter(
                                (ans, i) => {
                                    return ((
                                            this.questonsDto.answers[i].title.trim() != ans.title.trim() ||
                                            this.questonsDto.answers[i].isCorrect != ans.isCorrect
                                        ) && !ans.correctionDate)
                                }
                            )
                            .map(ans => {
                                return {
                                    title: ans.title,
                                    isCorrect: ans.isCorrect,
                                    correctAnswerId: ans.id
                                };
                            }),
                        ...this.questonsDto.answers
                    ]
                });
            }
        },
        getCorrectionQuestion() {
            Object.assign(this.localeQuestonsDto, {
                id: this.questonsDto.id,
                title: this.questonsDto.title,
                questionType: this.questonsDto.questionType,
                answerType: this.questonsDto.answerType,
                isCorrected: true,
                dateCreated: this.questonsDto.dateCreated,
                answers: this.questonsDto.answers.map(ans => ({ ...ans })),
                textAns:
                    this.mode && !this.typeTextAns
                        ? this.questonsDto.answers[this.correctAnswearIndex].title
                        : "",
                correctAnswearIndex: this.questonsDto.answers.findIndex(
                    ans => ans.isCorrect && !ans.correctionDate
                )
            });
        },
        resetCorrectQuestion() {
            Object.assign(this.localeQuestonsDto, {
                id: "",
                title: "",
                questionType: 0,
                answerType: 0,
                isCorrected: false,
                dateCreated: new Date(),
                answers: [],
                textAns: "",
                correctAnswearIndex: -1
            });
        },
        addQuestionLinks() {
            this.$refs.addQuestionLinks.validate().then(suc => {
                if (suc) {
                    this.selectedCourses.forEach(id => {
                        this.exams.unshift({
                            examId: id,
                            order: this.questionOrder,
                            examName: this.filterdCoursesBySubject.find(
                                course => course.id == id
                            ).name,
                            dateCreated: new Date()
                        });
                    });
                    this.selectedCourses = [];
                    this.selectedBanks.forEach(id => {
                        this.exams.unshift({
                            examId: id,
                            order: this.questionOrder,
                            examName: this.filterdBanksBySubject.find(
                                bank => bank.id == id
                            ).name,
                            dateCreated: new Date()
                        });
                    });
                    this.selectedBanks = [];
                    this.selectedInterviews.forEach(id => {
                        this.exams.unshift({
                            examId: id,
                            order: this.questionOrder,
                            examName: this.filterdInterviewsBySubject.find(
                                interview => interview.id == id
                            ).name,
                            dateCreated: new Date()
                        });
                    });
                    this.selectedInterviews = [];
                }
            });
        },
        onDelete() {
            this.deleteQuestion(this.questonsDto.id);
        },
        addAnswear() {
            if (this.newAns.trim()) {
                this.questonsDto.answers.push({
                    title: this.newAns,
                    isCorrect: false,
                    correctionDate: null
                });
                this.newAns = "";
            }
        },
        removeAnswear(index, anses) {
            if (index == this.correctAnswearIndex) {
                this.correctAnswearIndex = 0;
            } else if (index < this.correctAnswearIndex) {
                this.correctAnswearIndex -= 1;
            }
            anses.splice(index, 1);
        },
        setLocaleQuestion() {
            this.$refs.setLocaleQuestion.validate().then(suc => {
                if (suc &&
                    this.questonsDto.answers.length >= 2 &&
                    this.typeTextAns
                ) {
                     this.questonsDto.answers.forEach((ans) => {ans.isCorrect = false})
                    this.questonsDto.answers[
                        this.correctAnswearIndex
                    ].isCorrect = true;
                    if (!this.mode) {
                        let formData = new FormData();
                        formData.append("answerType", +this.ansType);
                        formData.append("questionType", 1);
                        formData.append(
                            "dateCreated",
                            new Date(
                                this.questonsDto.dateCreated
                            ).toLocaleDateString()
                        );
                        formData.append("hint", this.questonsDto.hint);
                        formData.append(
                            "isCorrected",
                            this.questonsDto.isCorrected
                        );
                        formData.append("order", this.questonsDto.order);
                        formData.append("title", this.questonsDto.title);

                        this.questonsDto.answers.forEach((ans, i) => {
                            formData.append(
                                "answers[" + i + "].title",
                                ans.title
                            );
                            formData.append(
                                "answers[" + i + "].isCorrect",
                                ans.isCorrect
                            );
                            formData.append(
                                "answers[" + i + "].correctionDate",
                                new Date(
                                    ans.correctionDate
                                ).toLocaleDateString()
                            );
                        });
                        this.questonsDto.images.forEach((file, i) => {
                            formData.append("images[" + i + "].file", file);
                            formData.append("images[" + i + "].note", " ");
                        });
                        this.questonsDto.exams.forEach((exam, i) => {
                            formData.append(
                                "exams[" + i + "].examId",
                                exam.examId
                            );
                            formData.append(
                                "exams[" + i + "].order",
                                exam.order
                            );
                            formData.append(
                                "exams[" + i + "].examName",
                                exam.examName
                            );
                            formData.append(
                                "exams[" + i + "].dateCreated",
                                new Date(exam.dateCreated).toLocaleDateString()
                            );
                        });
                        this.questonsDto.tags.forEach((tag, i) => {
                            formData.append("tags[" + i + "]", tag);
                        });
                        this.setQuestion(formData);
                    } else {
                        let formData = new FormData();
                        formData.append("id", this.questonsDto.id);
                        formData.append("answerType", +this.ansType);
                        formData.append("questionType", 1);
                        formData.append(
                            "dateCreated",
                            new Date(
                                this.questonsDto.dateCreated
                            ).toLocaleDateString()
                        );
                        formData.append("hint", this.questonsDto.hint);
                        formData.append(
                            "isCorrected",
                            this.questonsDto.isCorrected
                        );
                        formData.append("order", this.questonsDto.order);
                        formData.append("title", this.questonsDto.title);
                        this.questonsDto.answers.forEach((ans, i) => {
                            formData.append(
                                "answers[" + i + "].title",
                                ans.title
                            );
                            formData.append(
                                "answers[" + i + "].isCorrect",
                                ans.isCorrect
                            );
                            formData.append(
                                "answers[" + i + "].correctionDate",
                                new Date(
                                    ans.correctionDate
                                ).toLocaleDateString()
                            );
                        });
                        this.questonsDto.images.forEach((file, i) => {
                            if (file.file !== undefined) {
                                formData.append("images[" + i + "].sectionImageId", file.sectionImageId);
                                formData.append("images[" + i + "].path", file.path);
                                if (file.file) {
                                    formData.append("images[" + i + "].file", file.file);
                                }
                            } else {
                                formData.append("images[" + i + "].file", file);
                            }
                            formData.append("images[" + i + "].note", " ");
                        });
                        this.questonsDto.exams.forEach((exam, i) => {
                            formData.append(
                                "exams[" + i + "].examId",
                                exam.examId
                            );
                            formData.append(
                                "exams[" + i + "].order",
                                exam.order
                            );
                            formData.append(
                                "exams[" + i + "].examName",
                                exam.examName
                            );
                            formData.append(
                                "exams[" + i + "].dateCreated",
                                new Date(exam.dateCreated).toLocaleDateString()
                            );
                        });
                        this.questonsDto.tags.forEach((tag, i) => {
                            formData.append("tags[" + i + "]", tag);
                        });
                        this.updateQuestion(formData);
                    }
                } else if (!this.typeTextAns) {
                    // if(this.questonsDto.answers == null || this.questonsDto.answers.length == 0){
                    //     console.log("test")
                    //     this.questonsDto.answers = [];
                    //     this.questonsDto.answers.push(this.answer);
                    // }
                    if (this.mode) {
                        this.questonsDto.answers[this.correctAnswearIndex].title = this.textAns;
                    } else {
                        this.questonsDto.answers.push({
                            title: this.answer.title,
                            isCorrect: true,
                            correctionDate: null
                        });
                    }
                    if (!this.mode) {
                        let formData = new FormData();
                        formData.append("answerType", +this.ansType);
                        formData.append("questionType", 1);
                        formData.append(
                            "dateCreated",
                            new Date(
                                this.questonsDto.dateCreated
                            ).toLocaleDateString()
                        );
                        formData.append("hint", this.questonsDto.hint);
                        formData.append(
                            "isCorrected",
                            this.questonsDto.isCorrected
                        );
                        formData.append("order", this.questonsDto.order);
                        formData.append("title", this.questonsDto.title);
                        this.questonsDto.answers.forEach((ans, i) => {
                            formData.append(
                                "answers[" + i + "].title",
                                ans.title
                            );
                            formData.append(
                                "answers[" + i + "].isCorrect",
                                ans.isCorrect
                            );
                            formData.append(
                                "answers[" + i + "].correctionDate",
                                new Date(
                                    ans.correctionDate
                                ).toLocaleDateString()
                            );
                        });
                        this.questonsDto.images.forEach((file, i) => {
                            formData.append("images[" + i + "].file", file);
                            formData.append("images[" + i + "].note", " ");
                        });
                        this.questonsDto.exams.forEach((exam, i) => {
                            formData.append(
                                "exams[" + i + "].examId",
                                exam.examId
                            );
                            formData.append(
                                "exams[" + i + "].order",
                                exam.order
                            );
                            formData.append(
                                "exams[" + i + "].examName",
                                exam.examName
                            );
                            formData.append(
                                "exams[" + i + "].dateCreated",
                                new Date(exam.dateCreated).toLocaleDateString()
                            );
                        });
                        this.questonsDto.tags.forEach((tag, i) => {
                            formData.append("tags[" + i + "]", tag);
                        });
                        this.setQuestion(formData);
                    } else {
                        let formData = new FormData();
                        formData.append("id", this.questonsDto.id);
                        formData.append("answerType", +this.ansType);
                        formData.append("questionType", 1);
                        formData.append(
                            "dateCreated",
                            new Date(
                                this.questonsDto.dateCreated
                            ).toLocaleDateString()
                        );
                        formData.append("hint", this.questonsDto.hint);
                        formData.append(
                            "isCorrected",
                            this.questonsDto.isCorrected
                        );
                        formData.append("order", this.questonsDto.order);
                        formData.append("title", this.questonsDto.title);
                        this.questonsDto.answers.forEach((ans, i) => {
                            formData.append(
                                "answers[" + i + "].title",
                                ans.title
                            );
                            formData.append(
                                "answers[" + i + "].isCorrect",
                                ans.isCorrect
                            );
                            formData.append(
                                "answers[" + i + "].correctionDate",
                                new Date(
                                    ans.correctionDate
                                ).toLocaleDateString()
                            );
                        });
                        this.questonsDto.images.forEach((file, i) => {
                            if (file.file !== undefined) {
                                formData.append("images[" + i + "].sectionImageId", file.sectionImageId);
                                formData.append("images[" + i + "].path", file.path);
                                if (file.file) {
                                    formData.append("images[" + i + "].file", file.file);
                                }
                            } else {
                                formData.append("images[" + i + "].file", file);
                            }
                            formData.append("images[" + i + "].note", " ");
                        });
                        this.questonsDto.exams.forEach((exam, i) => {
                            formData.append(
                                "exams[" + i + "].examId",
                                exam.examId
                            );
                            formData.append(
                                "exams[" + i + "].order",
                                exam.order
                            );
                            formData.append(
                                "exams[" + i + "].examName",
                                exam.examName
                            );
                            formData.append(
                                "exams[" + i + "].dateCreated",
                                new Date(exam.dateCreated).toLocaleDateString()
                            );
                        });
                        this.questonsDto.tags.forEach((tag, i) => {
                            formData.append("tags[" + i + "]", tag);
                        });
                        this.updateQuestion(formData);
                    }
                }
            });
        }
    },
    watch: {
        "questionFilterDto.subjectID"() {
            this.selectedBanks = [];
            this.selectedCourses = [];
            this.selectedInterviews = [];
        },
        "questonsDto.answers"(anses) {
            if (!this.typeTextAns && anses[0]) 
                anses.forEach((ans, index) => {
                    if (ans.isCorrect && !ans.correctionDate) {
                        this.correctAnswearIndex = index
                        this.textAns = ans.title;
                    }
                });
            else
                anses.forEach((ans, index) => {
                    if (ans.isCorrect && !ans.correctionDate) this.correctAnswearIndex = index;
                });
        }
    }
};
</script>

<style lang="scss">
.accordion-question-anwser .collapse .card-body {
    padding: 1rem 0 !important;
}
</style>

import {getSearchResultOnRow} from "@Ekcore/util"
export default {
    state: {
        filterDto: {
            semesterId:0,
            year: 0,
            facultyId: 0,
            examYear: 0,
            subjectId: 0
        },
        searchDto: {
            keys: [],
            query: ''
        }
    },
    getters: {
        facultiesList(state, getter, glState) {
            return glState.faculties.faculties.filter(facul => {
                return getSearchResultOnRow(state, facul)
            })
        },
        subjectsList(state, getter, glState) {
            return glState.subjects.subjectsList.filter(subject => {
                return (
                    getSearchResultOnRow(state, subject) &&
                    (subject.facultyId == state.filterDto.facultyId ||
                        !state.filterDto.facultyId) &&
                    (subject.semesterId == state.filterDto.semesterId ||
                        !state.filterDto.semesterId) &&
                    (subject.year == state.filterDto.year ||
                        !state.filterDto.year) 
                );
            });
        },
        courcesList(state, getter, glState) {
            return glState.cources.courcesList.filter(cource => {
                return (
                    getSearchResultOnRow(state, cource) &&
                    (cource.semesterId == state.filterDto.semesterId ||
                        !state.filterDto.semesterId) &&
                    // (cource.subject.facultyId == state.filterDto.facultyId ||
                    //     !state.filterDto.facultyId) &&
                    (cource.year == state.filterDto.examYear ||
                        !state.filterDto.examYear) &&
                    (cource.subjectId == state.filterDto.subjectId ||
                        !state.filterDto.subjectId)
                );
            });
        },
        searchedCourcesQuestionList(state, getter, glState) {
            return glState.cources.courcesQuestionList.questions.filter(ques => {
                return getSearchResultOnRow(state, ques)
            })
        },
        banksList(state, getter, glState) {
            return glState.banks.banksList.filter(bank => {
                return (
                    getSearchResultOnRow(state, bank) &&
                    (bank.semesterId == state.filterDto.semesterId ||
                        !state.filterDto.semesterId) &&
                    (bank.subject.facultyId == state.filterDto.facultyId ||
                        !state.filterDto.facultyId) &&
                    (bank.year == state.filterDto.examYear ||
                        !state.filterDto.examYear) &&
                    (bank.subjectId == state.filterDto.subjectId ||
                        !state.filterDto.subjectId)
                );
            });
        },
        searchedBanksQuestionList(state, getter, glState) {
            return glState.banks.banksQuestionList.questions.filter(ques => {
                return getSearchResultOnRow(state, ques)
            })
        },
        interviewsList(state, getter, glState) {
            return glState.interviews.interviewsList.filter(interview => {
                return (
                    getSearchResultOnRow(state, interview) &&
                    (interview.semesterId == state.filterDto.semesterId ||
                        !state.filterDto.semesterId) &&
                    (interview.subject.facultyId == state.filterDto.facultyId ||
                        !state.filterDto.facultyId) &&
                    (interview.year == state.filterDto.year ||
                        !state.filterDto.year) &&
                    (interview.subjectId == state.filterDto.subjectId ||
                        !state.filterDto.subjectId)
                );
            });
        },
        searchedInterviewQuestionList(state, getter, glState) {
            return glState.interviews.interviewQuestionList.questions.filter(ques => {
                return getSearchResultOnRow(state, ques)
            })
        },
        telescopeList(state, getter, glState) {
            return glState.telescope.telescopeList.filter(telescope => {
                return (
                    getSearchResultOnRow(state, telescope) &&
                    (telescope.semesterId == state.filterDto.semesterId ||
                        !state.filterDto.semesterId) &&
                    (telescope.year == state.filterDto.examYear ||
                        !state.filterDto.examYear)
                );
            });
        },
        questonsList(state, getter, glState) {
            return glState.questions.questonsList.filter(queton => {
                return (
                    getSearchResultOnRow(state, queton) &&
                    (queton.semesterId == state.filterDto.semesterId ||
                        !state.filterDto.semesterId) &&
                    (queton.year == state.filterDto.examYear ||
                        !state.filterDto.examYear) &&
                    (queton.subjectId == state.filterDto.subjectId ||
                        !state.filterDto.subjectId)
                );
            });
        },
        advertising(state, getter, glState) {
            return glState.advertising.advertising.filter(ad => {
                return getSearchResultOnRow(state, ad)
            })
        },
        notificationList(state, getter, glState) {
            return glState.natification.notificationList.filter(notifi => {
                return getSearchResultOnRow(state, notifi)
            })
        },
        codesList(state, getter, glState){
            return glState.codes.codesList.filter(code => {
                return getSearchResultOnRow(state, code)
            }) 
        },
        packagesList(state, getter, glState){
            return glState.packages.packagesList.filter(pack => {
                return getSearchResultOnRow(state, pack)
            }) 
        },
        usersInvoicesList(state, getter, glState){
            return glState.invoices.usersInvoicesList.filter(pack => {
                return getSearchResultOnRow(state, pack)
            }) 
        },
        invoicesList(state, getter, glState){
            return glState.invoices.invoicesList.filter(pack => {
                return getSearchResultOnRow(state, pack)
            }) 
        },
        usersList(state, getter, glState){
            return glState.accounts.usersList.filter(pack => {
                
                    return getSearchResultOnRow(state, pack) 
                    //  (pack.faculties.findIndex(fa => fa.id == state.filterDto.facultyId) != -1 ||!state.filterDto.facultyId)
               
              
                 
            }) 
        }
    },
    mutations: {
        Set_filter_Dto(state, payload) {
            Object.keys(payload).forEach((key) => {
                state.filterDto[key] = payload[key]
            })
        },
        Set_Search_Dto(state, payload) {
            Object.assign(state.searchDto, payload)
        },
        Reset_Search_Dto(state) {
            Object.assign(state.searchDto, {
                keys: [],
                query: ''
            })
        },
        Reset_filter_Dto(state) {
            Object.assign(state.filterDto, {
                semesterId:0,
                year: 0,
                facultyId: 0,
                examYear: 0,
                subjectId: 0
            })
        },
    }
}


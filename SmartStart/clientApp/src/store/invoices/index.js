import api from "@api";
import router from "@/router";

export default {
    state: {
        posName: "",
        usersInvoicesList: [],
        invoicesList: [],
        invoiceDto: {
            invoiceId: "",
            posId: "",
            invoiceNumber: "",
            date: new Date(),
            codeCount: 0,
            actualCost: 0,
            dueCompany: 0,
            paidMoney: 0,
            notes: "",
            posName: '',
            codes: []
        }
    },
    mutations: {
        Get_Users_Invoice(state, payload) {
            state.usersInvoicesList = payload;
        },
        Get_User_Invoices_By_Id(state, payload) {
            state.posName = payload.name;
            state.invoicesList = payload.data;
        },
        Fill_Invoice(state, data) {
            Object.assign(state.invoiceDto, data);
            if (data.invoiceId == "00000000-0000-0000-0000-000000000000") state.invoiceDto.paidMoney = 0
        }
    },
    actions: {
        getUsersInvoice({ commit }) {
            api.get("Invoice/GetUsersDetails", ({ data }) => {
                commit("Get_Users_Invoice", data);
            });
        },
        getUserInvoicesById({ commit }, id) {
            api.get("Invoice/GetInvoicesByPosId?posId=" + id, ({ data }) => {
                commit("Get_User_Invoices_By_Id", { name: data.name, data: data.invoices });
            });
        },
        createInvoice(ctx, payload) {
            api.post('Invoice/CreateInvoice', payload.dto, ({data}) => {
                if (payload.isPrint) {
                    router.push("/invoice/" + data.posId + "/print/" + data.invoiceId);
                } else {
                    router.push("/invoice/" + data.posId)
                }
            },{
                success:"تم إضافة الفاتورة بنجاح",
                error:"فشل إضافة الفاتورة"
            })
        },
        fillInvoice({ commit }, posId) {
            api.get("Invoice/FillInvoice?posId=" + posId, ({ data }) => {
                commit("Fill_Invoice", data);
            });
        },
        getInvoiceById({ commit }, invoiceId) {
            api.get(
                "Invoice/GetInvoiceById?invoiceId=" + invoiceId,
                ({ data }) => {
                    commit("Fill_Invoice", data);
                }
            );
        },
        // TODO:
        deleteInvoice(ctx, id) {
            api.delete("Invoice/DeleteInvoice?invoiceId=" + id, ({ data }) => {
                if (data.isSuccess) {
                    router.push("/invoice" + id);
                }
            }
            ,{
                confirm:"هل تريد فعلا حذف  الحساب",
                success:"تم حذف الحساب بنجاح",
                error:"فشل حذف الحساب الحساب"
            });
        }
    }
};

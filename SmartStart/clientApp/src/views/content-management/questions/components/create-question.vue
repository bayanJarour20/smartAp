<template>
<activaitor @ok="goToAdd" @search="search" placeholder="ابحث عن سؤال محدد" btnText="سؤال جديد" />
</template>
<script>
import activaitor from "@Ekcore/components/EK-dialog/activaitor";
export default {
    components: {
        activaitor
    },
    methods: {
        goToAdd() {
            this.$router.push("/questions/1/set/0/0/0")
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: ['title', 'hint'],
                query   
            })
        }
    }
};
</script>

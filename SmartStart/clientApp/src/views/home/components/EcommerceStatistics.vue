<template>
  <b-card
    v-if="data"
    no-body
    class="card-statistics"
  >
    <b-card-header>
      <b-card-title>الإحصائيات</b-card-title>
    </b-card-header>
    <b-card-body class="statistics-body">
      <b-row>
        <b-col
          xl="3"
          sm="6"
        >
          <b-media no-body>
            <b-media-aside
              class="mr-2"
            >
               <b-avatar
                size="48"
                variant="light-primary"
              >
                <feather-icon
                  size="24"
                  icon="BookIcon"
                />
              </b-avatar>
            </b-media-aside>
            <b-media-body class="my-auto">
              <h4 class="font-weight-bolder mb-0">
                {{ data.subjectCount }}
              </h4>
              <b-card-text class="font-small-3 mb-0">
                عدد المواد
              </b-card-text>
            </b-media-body>
          </b-media>
        </b-col>
        <b-col
          xl="3"
          sm="6"
        >
          <b-media no-body>
            <b-media-aside
              class="mr-2"
            >
            <b-avatar
                size="48"
                variant="light-primary"
              >
                <feather-icon
                  size="24"
                  icon="ClipboardIcon"
                />
              </b-avatar>
            </b-media-aside>
            <b-media-body class="my-auto">
              <h4 class="font-weight-bolder mb-0">
                {{ data.unitCount }}
              </h4>
              <b-card-text class="font-small-3 mb-0">
                عدد الوحدات
              </b-card-text>
            </b-media-body>
          </b-media>
        </b-col>
        <b-col
          xl="3"
          sm="6"
        >
          <b-media no-body>
            <b-media-aside
              class="mr-2"
            >
                <b-avatar
                size="48"
                variant="light-primary"
              >
                <feather-icon
                  size="24"
                  icon="FileTextIcon"
                />
              </b-avatar>
            </b-media-aside>
            <b-media-body class="my-auto">
              <h4 class="font-weight-bolder mb-0">
                {{ data.lessonCount }}
              </h4>
              <b-card-text class="font-small-3 mb-0">
                عدد الدروس
              </b-card-text>
            </b-media-body>
          </b-media>
        </b-col>
        <b-col
          xl="3"
          sm="6"
        >
          <b-media no-body>
            <b-media-aside
              class="mr-2"
            >
             <b-avatar
                size="48"
                variant="light-primary"
              >
                <feather-icon
                  size="24"
                  icon="VideoIcon"
                />
              </b-avatar>
            </b-media-aside>
            <b-media-body class="my-auto">
              <h4 class="font-weight-bolder mb-0">
                {{ data.videoCount }}
              </h4>
              <b-card-text class="font-small-3 mb-0">
                عدد الفيديوهات
              </b-card-text>
            </b-media-body>
          </b-media>
        </b-col>
      </b-row>
    </b-card-body>
  </b-card>
</template>

<script>
import {
  BCard, BCardHeader, BCardTitle, BCardText, BCardBody, BRow, BCol, BAvatar , BMedia, BMediaAside, BMediaBody,
} from 'bootstrap-vue'

export default {
  components: {
    BRow,
    BCol,
    BCard,
    BCardHeader,
    BCardTitle,
    BCardText,
    BCardBody,
    BMedia,
    BAvatar,
    BMediaAside,
    BMediaBody,
  },
  props: {
    data: {
      type: Object,
      default: () => [],
    },
  },
}
</script>

<template>
  <v-container class="ticketColumn">
 
    <v-row style="justify-content: left; margin: 20px;">
      <h3>{{ translator("ticketGroups") }}</h3>
    </v-row>

    <v-row
      v-for="(item, index) in orderedTicketGroup" :key="index + item"
      class="newsRow elevation-3"
      style="justify-content: center;">
      <v-col cols="12" xl="8" md="8" sm="12">
        <v-layout class="d-md-block">
          <div class="text-justify">
            <div class="headline">
              <span style="font-size: large; color: black;">
                <strong>{{ item.title }}</strong>
              </span>
              <div class="subtitle-2 date">
                <span style="font-size: smaller; color: black;"> Ticket Id : {{ item.id }} </span>
              </div>
            </div>
          </div>
        </v-layout>
      </v-col>

      <v-col>
        <v-layout class="d-md-block" style="text-align-last: right;">
          <div class="text-justify mt-3" style="display: inline !important;">
            <b> {{ new Date(item.createdDate).toISOString().substr(0, 10).replace("T", " ") }} </b>
                {{ new Date(item.createdDate).toISOString().substr(10, 6).replace("T", " ") }}

            <v-switch
              v-model="item.isClosed"
              @change="changeClosed(item)"
              style="display: inline-flex !important;"
              required
              outlined
            ></v-switch>

            <v-icon md class="mr-2 reply ml-5" @click="ticketMessageDetails(item)">
              {{ item.isRead === false ? "mdi-email-alert " : "mdi-email-open" }}
            </v-icon>
          </div>
        </v-layout>
      </v-col>
    </v-row>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>

  </v-container>
</template>

<script>
import TicketService from "@/services/TicketService";
import _ from "lodash";

/* eslint-disable */
export default {
  data() {
    return {
      ticketGroup: [],
      ticketGroupModel: {
        company: "",
        userName: "",
        userId: 0,
      },
      ticketGroupId: 0,

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: 400,
      selectedComponent: "",
      componentProps: null,
      signalModel: {
        changes: false,
        returnValues: null,
      },
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: -1,
      },

      loading: true,
    };
  },
  created() {
    this.initialize();
  },
  methods: {
    async initialize() {
      this.loading = true;
      this.ticketGroup = [];

      let response = await new TicketService().getListAll().finally(() => {
        this.loading = false;
      });
      this.ticketGroup = response.list;
    },
    async ticketMessageDetails(ticketMessage) {
      await new TicketService().getGroupReadInfo(ticketMessage.id);
      this.$router.push({
        path: "/admin/ticket-details/groupId=" + ticketMessage.id + "&userId=" + ticketMessage.userId,
      });
    },
    async changeClosed(ticketGroup) {
      await new TicketService().isClosedChange(ticketGroup.id);
    },
    openDialog(width = 600) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
  },
  computed: {
    orderedTicketGroup() {
      return _.orderBy(this.ticketGroup, ["createdDate"], ["desc"]);
    },
  },
};
</script>

<style scoped>
.ticketColumn {
  width: 60%;
}
.newsRow {
  background: #eeeeee;
  margin: 30px 2% 30px 2%;
}
</style>

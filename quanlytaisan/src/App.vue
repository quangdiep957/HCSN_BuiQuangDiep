<template>
  <div class="container">
    <Navbar />
    <Body />
  </div>
</template>

<script>
import { VueCookieNext } from "vue-cookie-next";
import router from "./router";
import Navbar from "./components/layouts/TheNavbar.vue";
import Body from "./components/layouts/TheBody.vue";
import { API } from "@/js/callApi";
import Resource from "@/js/resource";
export default {
  name: "QuanlytaisanAssetView",
  components: {
    Navbar,
    Body,
  },
  data() {
    return {};
  },

  methods: {
    /**
     * Chuyển hướng trang khi đã đăng nhập
     * created By : Bùi Quang Điệp
     * Date:27/09/2022
     */
    redirectRouter() {
      try {
        // Kiểm tra xem token có rỗng hay không nếu rỗng thì chuyển sang trang đăng nhập
        var checkCookie = VueCookieNext.getCookie("login");
        if (checkCookie != null) {
          router.push("asset");
        } else {
          router.push("login");
        }
      } catch (error) {
        console.log(error);
      }
    },
  },
  created() {
    API.get(Resource.APIs.UserCheck)
      .then((res) => {
        console.log(res);
      })
      .catch((error) => {
        console.log(error);
        router.push("login");
      });
    this.redirectRouter();
  },
  mounted() {
    this.emitter.on("redirect", () => {
      this.redirectRouter();
    });

    this.emitter.on("dateFormat", (item) => {
      Resource.FormatDate.Value = item;
      var a = Resource.FormatDate.Value;
      console.log(a);
    });
  },
};
</script>

<style scoped></style>

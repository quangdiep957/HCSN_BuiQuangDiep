<template>
    <div class="m-row managetable">
        <div class="table__nodata" v-if="isNoData">
            <div class="table__nodata--item">

                <div>Dữ liệu hiện tại đang trống</div>
                <img style="width: 200px;height: 200px;"
                    src="../../assets/icon/empty-concept-illustration_114360-1233.webp">
            </div>
        </div>
        <table class="table">
            <thead>
                <tr>
                    <th><button class="resize-width"><label class="checkbox"><input @click="isTicksAll()"
                                    type="checkbox" v-model="checked"><span class="tick"></span></label></button></th>
                    <th class="text-align-center tooltip--th "><button class="resize-width b tooltip">STT
                            <Tooltip tooltiptext="Số thứ tự" positiontooltip="top" style="margin-top:10px ;" />
                        </button> </th>
                    <th><button class="resize-width text-align-left">Mã tài sản</button></th>
                    <th><button class="resize-width text-align-left">Tên tài sản</button></th>
                    <th><button class="resize-width text-align-left">Loại sản phẩm </button></th>
                    <th><button class="resize-width text-align-left">Bộ phận sử dụng </button></th>
                    <th><button class="resize-width text-align-right ">Số lượng </button></th>
                    <th><button class="resize-width text-align-right ">Nguyên giá </button></th>
                    <th tooltip--th><button class=" text-align-right resize-width tooltip">HM/KH lũy kế
                            <Tooltip tooltiptext="Hao mòn/khấu hao lũy kế" positiontooltip="top"
                                style="margin-top:10px" />
                        </button></th>
                    <th><button class="resize-width text-align-right ">Giá trị còn lại </button></th>
                    <th><button class="resize-width  text-align-center">Chức năng </button></th>
                </tr>

            </thead>
            <tbody>

                <tr v-contextmenu:contextmenu v-for="(item, index) in Asset" :key="item.fixedAssetID"
                    @click="selectTick($event, item, index)" draggable="true" @dragstart="startDrag($event, item)"
                    :class="{ 'table-active': checkTick(item) }" @dblclick="isShowDialogDetail(item)">
                    <td > <label class="checkbox"><input @click="selectTick($event, item, index)" :checked="checkTick(item)" type="checkbox"><span
                                class="tick"></span></label></td>
                    <td class="text-align-center">{{ index + 1 }}</td>
                    <td class="text-align-left" id="IdAsset">{{ item.fixedAssetCode }}</td>
                    <td class="text-align-left" id="nameAsset">{{ item.fixedAssetName }}</td>
                    <td class="text-align-left">{{ NameCategoryAsset(item.fixedAssetCategoryID) }}</td>
                    <td class="text-align-left">{{ NameDepartment(item.departmentID) }}</td>
                    <td class="text-align-right quantity">{{ item.quantity }}</td>
                    <td class="text-align-right price">{{ formatCash(item.cost) }}</td>
                    <td class="text-align-right Atrophy">{{ (item.depreciationRate) }}</td>
                    <td class="text-align-right price-remaining">{{ formatCash(item.trackedYear) }}</td>
                    <td class="text-align-center">
                        <div class="tablefunction display-flex">
                            <div @click="isShowDialogDetail(item)"
                                class="icon-mini icon-edit-mini icon__size-14 tooltip tooltipHandler"
                                style="margin-left:15px">
                                <Tooltip tooltiptext="Sửa" positiontooltip="top" style="top: 20px;" />
                            </div>
                            <div @click="ReplicationAsset(item)"
                                class="icon-mini icon-replication-mini icon__size-14 tooltip tooltipHandler tooltipHandlerEdit"
                                style="margin-left:10px">
                                <Tooltip tooltiptext="Nhân bản" positiontooltip="top"
                                    style="margin-left:-33px !important;top: 20px" />
                            </div>
                            <div class="icon-mini icon-remove-mini icon__size-14 tooltip tooltipHandler tooltipHandlerRemove"
                                @click="HandleRemoveDetail(item)" style="margin-left:10px">
                                <Tooltip tooltiptext="Xóa" positiontooltip="top"
                                    style="top: 20px;margin-left:-18px !important" />
                            </div>
                        </div>
                    </td>

                </tr>
                <tr style="height:0px;border:none"></tr>
                <tr class="page__table">
                    <td class="text-align-left" colspan="6">
                        <div class="border-paging"></div>
                    </td>
                    <td class="text-align-right sum-quantity bold">
                        <div class="border-paging">{{ formatCash(sumQuantity) }}</div>
                    </td>
                    <td class="text-align-right sum-price bold">
                        <div class="border-paging">{{ computedSumPrice }}</div>
                    </td>
                    <td class="text-align-right sum-Atrophy bold">
                        <div class="border-paging">{{ formatCash(sumDepreciation) }}</div>
                    </td>
                    <td class="text-align-right sum-price-rimaining bold">
                        <div class="border-paging">{{ formatCash(sumAtrophy) }}</div>
                    </td>
                    <td>
                        <div class="border-paging" style=" width: 100%;"></div>
                    </td>
                </tr>

            </tbody>
        </table>

    </div>
    <div class="display-flex paging">
        <div class="display-flex paging_item">
            <p class="page__table--text">Tổng số : <span style="font-weight: bold;">{{ sumItem }}</span>
                bản ghi</p>
            <Combobox ComboboxQuantity="true" style="width:100px" />
            <div class="page__table--number display-flex">
                <div class="page--number-item">
                    <div class="icon icon-back icon__size-8 tooltip" @click="BackPage">
                        <Tooltip tooltiptext="Trang trước" positiontooltip="bottom" />
                    </div>
                </div>
                <div v-for="item in numberPageArray" :key="item" class="page--number-item" @click="BtnPageNumber(item)" :class="{'number-active' : item==CheckNumber(item)}">{{ item }}</div>

                <div class="page--number-item">
                    <div @click="NextPage" class="icon icon-next icon__size-8 tooltip">
                        <Tooltip tooltiptext="Trang sau" positiontooltip="bottom" />
                    </div>
                </div>
            </div>
        </div>


    </div>



    <!-- <div class="menu">
        <div class="menu__item" v-if="isShowContentMenu">
            <div class="menu__item--icon icon icon-add-menu icon__size-18"></div>
            <div class="menu__item--text">Thêm</div>
        </div>
        <div class="menu__item">
            <div class="menu__item--icon icon icon-edit icon__size-18" style="margin:0"></div>
            <div class="menu__item--text">Sửa</div>
        </div>
        <div class="menu__item">
            <div class="menu__item--icon icon icon-remove icon__size-18"></div>
            <div class="menu__item--text">Xóa</div>
        </div>
        <div class="menu__item">
            <div class="menu__item--icon icon icon-replication icon__size-18"></div>
            <div class="menu__item--text">Nhân bản</div>
        </div>
    </div> -->
    <v-contextmenu ref="contextmenu">
        <v-contextmenu-item>
            <div class="menu__item">
                <div class="menu__item--icon icon icon-add-menu icon__size-18"></div>
                <div class="menu__item--text">Thêm</div>
            </div>
        </v-contextmenu-item>
        <v-contextmenu-item>
            <div class="menu__item">
                <div class="menu__item--icon icon icon-edit icon__size-18" style="margin:0"></div>
                <div class="menu__item--text">Sửa</div>
            </div>
        </v-contextmenu-item>
        <v-contextmenu-item>
            <div class="menu__item">
                <div class="menu__item--icon icon icon-remove icon__size-18"></div>
                <div class="menu__item--text">Xóa</div>
            </div>
        </v-contextmenu-item>
        <v-contextmenu-item>
            <div class="menu__item">
                <div class="menu__item--icon icon icon-replication icon__size-18"></div>
                <div class="menu__item--text">Nhân bản</div>
            </div>
        </v-contextmenu-item>
    </v-contextmenu>
    <Notify v-bind:dataError=titleWarning v-if="isShowDialogNotify" />

    <div class="m-loading" v-show="isLoading">
        <div class="loader"></div>
    </div>
</template>

<script>
import { formatCash } from '../../js/common.js'
import { directive, Contextmenu, ContextmenuItem } from "v-contextmenu";
import "v-contextmenu/dist/themes/default.css";
import Notify from '../base/TheDialogNotify.vue'
import Tooltip from '../base/TheTooltip.vue'
import axios from 'axios'
import Combobox from '../base/TheComboBox.vue'
export default {
    name: 'QuanlytaisanTheTable',
    directives: {
        contextmenu: directive,
    },
    components: {
        [Contextmenu.name]: Contextmenu,
        [ContextmenuItem.name]: ContextmenuItem, Tooltip, Combobox,
        Notify
    },
    data() {
        return {
            isLoading: false,
            directives: {
                contextmenu: directive,
                // isLoading :false,
                isNoData: false,
            },
            components: {
                [Contextmenu.name]: Contextmenu,
                [ContextmenuItem.name]: ContextmenuItem,
            },
            isClickRow: "",
            Asset: [],
            sumQuantity: 0,
            sumPrice: 0,
            sumDepreciation: 0,
            sumAtrophy: 0,
            isShowContentMenu: false,
            show: false,
            dataTicks: [],
            IndexFirst: 0,
            IndexLast: 0,
            sumItem: 0,
            titleWarning: [],
            isShowDialogNotify: false,
            dataDepartment: [],
            dataCategory: [],
            searchArray: {},
            assetCode: "",
            checked: false,
            recordNumber: "10",
            numberPage: "10",
            numberPageArray: [],
            numberTwo: 0,
            isCheckNumber:1
        };
    },
    computed: {
        computedSumPrice() {
            return formatCash(this.sumPrice)
        },



    },

    methods: {

        /**
         * 
         */
        formatCash,
        /**
        *  Truyền dữ liệu 1 hàng vào 1 mảng
        * Author : Bùi Quang Điệp
        * Date:10/08/2022
        */
        btnRowTable(item) {
            console.log(item);
        },
        isShowDialogDetail(item) {

            this.emitter.emit("itemDialog", item);
            this.emitter.emit("handlerEdit");
        },
        /**
         * Xử lý sự kiện nhấn phím ctrl thì chọn nhiều
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         * @param(e,item,index) e là sự kiện được click vào
         *            item là truyền row vừa click trên table
         *            index là số thứ tự của row đó trong table 
         */
        selectTick(e, item, index) {
            try {
                // Kiểm tra xem mã tài sản mới đã được thêm vào table chưa 
                // Nếu đã có thì focus vào dòng đó
                if (this.assetCode != "") {


                    for (let i = 0; i < this.Asset.length; i++) {
                        if (this.assetCode == this.Asset[i].fixedAssetCode) {
                            this.dataTicks.push(this.Asset[i]);
                        }
                    }
                }
                // Kiểm tra xem có giữ phím ctrl hay không
                // Nếu có thì thêm vào mảng và không reset lại mảng 
                // Ngược lại thì reset mảng
                if (e.ctrlKey == true) {
                    this.dataTicks.push(item);

                }
                /**
                * Xử lý sự kiện nhấn phím Shitft  thì chọn hàng loạt đến vị trí click
                * Author : Bùi Quang Điệp
                * Date:10/08/2022
                * */
                else if (e.shiftKey == true) {
                    this.IndexLast = index;
                    var tg = "";
                    // Kiểm tra vị trí nếu vị trí click bên trên thì đổi ngược lại vị trí index
                    if (this.IndexLast < this.IndexFirst) {
                        tg = this.IndexLast;
                        this.IndexLast = this.IndexFirst;
                        this.IndexFirst = tg;

                    }
                    // Gán số lượng được chọn về rỗng
                    this.dataTicks = [];
                    for (var i = this.IndexFirst; i <= this.IndexLast; i++) {
                        this.dataTicks.push(this.Asset[i]);
                    }

                }
                else {
                    // Kiểm tra nếu click vào 1 tài sản đã chọn thì hủy tick tài sản đó
                    if (this.dataTicks[0] == item) {
                        this.dataTicks = [];
                    }
                    // Nếu không phải thì xóa tài sản trước đó và thêm mới tài sản vừa chọn
                    else {
                        this.dataTicks = [];
                        this.dataTicks.push(item);
                        this.IndexFirst = index;
                    }

                    // Kiểm tra nếu số lượng tick hết = số lượng bản ghi đang hiển thị thì tick all
                }
                if (this.dataTicks.length == this.Asset.length) {
                    this.checked = true;
                }
                else {
                    this.checked = false;
                }
                this.emitter.emit("titleWarning", this.dataTicks);
            } catch (error) {
                console.log(error);
            }

        },
        /**
        * Kiểm tra xem row nào đã được tick
        * Author : Bùi Quang Điệp
        * Date:10/08/2022
        */
        checkTick(item) {
            try {

                for (const value of this.dataTicks) {
                    if (item == value) {
                        return item;
                    }
                }

            } catch (error) {
                console.log(error);
            }
        },
        /**
        * Tick tất cả 
        * Author : Bùi Quang Điệp
        * Date:10/08/2022
        */
        isTicksAll() {
            this.checked = !this.checked;
            if (this.checked == true) {
                for (const item of this.Asset) {
                    this.dataTicks.push(item);
                }
            }
            else {
                this.dataTicks = [];
                return
            }
        },

        /**
         * Xử lý sự kiện xóa
         * Author : Bùi Quang Điệp
         * Date:10/08/2022
         */

        HandleRemoveDetail(item) {
            this.isShowDialogNotify = true;
            this.titleWarning.push(`Bạn có thật sự muốn xóa tài sản ${item.fixedAssetName} này không ?`);

        },

        /**
        * Lấy tên bộ phận sử dụng
        * Author : Bùi Quang Điệp
        * Date:17/08/2022
        */
        NameDepartment(id) {

            for (const item of this.dataDepartment) {
                if (item.DepartmentID == id) {
                    return item.DepartmentName;
                }
            }
        },

        /**
        * Tính số lượng trang
        * Author : Bùi Quang Điệp
        * Date:17/08/2022
        */
        NumberPage(sumItem) {
            if (sumItem != undefined) {

                this.numberPage = sumItem / this.recordNumber;
            }

        },

        /**
        * Chuyển trang tiếp theo
        * Author : Bùi Quang Điệp
        * Date:17/08/2022
        */
        NextPage() {
            // sử dụng để tính toán biến i trong vòng for
            let number = 1;
            if (this.numberTwo <= this.numberPage) {
                // Nếu trường hợp người dùng đã chọn next trang thì sẽ gán lại biến i tăng lên 2
                if (this.numberTwo >= 2) {
                    number = this.numberTwo;
                }
                // mặc định biến i = 1
                else {
                    number = 1;
                }
                // Lấy 2 số tiếp theo 
                this.numberTwo = this.numberTwo + 2;
                this.numberPageArray = [];

                // lấy 2 số đầu gần nhất
                for (let i = number; i <= this.numberPage; i++) {

                    if (i < this.numberTwo) {
                        this.numberPageArray.push(i);
                    }
                }
                //  Những số k hiển thị để thành dấu ...
                this.numberPageArray.push("...");
                // lấy 2 số sau gần nhất
                for (let i = 1; i <= this.numberPage; i++) {
                    if (i > this.numberPage - 2) {
                        this.numberPageArray.push(i);
                    }
                }
            }
            else {
                this.numberTwo = 0
            }

        },

        /**
         * Chuyển trang về phái trước
         * Author : Bùi Quang Điệp
         * Date:17/08/2022
         */
        BackPage() {
            debugger
            let number = 1;
            if(this.numberTwo!=0)  
            {
                number = this.numberTwo - 4;  
              this.numberPageArray = [];
              this.numberTwo=this.numberTwo-2;
                // lấy 2 số đầu gần nhất
                for (let i = number; i <=this.numberPage; i++) { 

                    if (i < this.numberTwo) {  
                        this.numberPageArray.push(i);
                    }
                }
                //  Những số k hiển thị để thành dấu ...
                this.numberPageArray.push("...");
                // lấy 2 số sau gần nhất
                for (let i = 1; i <= this.numberPage; i++) {
                    if (i > this.numberPage - 2) {
                        this.numberPageArray.push(i);
                    }
                }
            }
            

        },

         /**
         * chọn số trang muốn hiển thị
         * Author : Bùi Quang Điệp
         * Date:17/08/2022
         */
        BtnPageNumber(item){

            this.isCheckNumber = item;
            // truyền số trang muốn hiển thị
            this.emitter.emit("pageNumber",item);


        },


        /**
         * Lấy tên loại tài sản
         * Author : Bùi Quang Điệp
         * Date:17/08/2022
         */
        NameCategoryAsset(id) {


            for (const item of this.dataCategory) {
                if (item.FixedAssetCategoryID == id) {
                    return item.FixedAssetCategoryName;
                }
            }
        },
        /*
         * Hàm lấy dữ liệu đưa vào table
         * Author : Bùi Quang Điệp
         * Date : 14/08/2022
         */

        getAll() {
            var httpSearch = `http://localhost:13846/api/v1/FixedAssets/filter?`;
            if (this.searchArray.keyword != undefined) {
                if (this.searchArray.keyword != "") {
                    httpSearch = httpSearch + `keyword=${this.searchArray.keyword}&`;
                }
                if (this.searchArray.fixedAssetCategoryID != "") {
                    httpSearch = httpSearch + `categoryAssetID=${this.searchArray.fixedAssetCategoryID}&`;
                }
                if (this.searchArray.departmentID != "") {
                    httpSearch = httpSearch + `departmentID=${this.searchArray.departmentID}&`;
                }
                if (this.searchArray.pageSize != "") {
                    httpSearch = httpSearch + `pageSize=${this.searchArray.pageSize}&`;
                }
                if(this.searchArray.pageNumber != "")
                {
                    httpSearch = httpSearch + `pageNumber=${this.searchArray.pageNumber}`;
                }

            }
            this.isLoading = true;
            axios.get(httpSearch)
                .then(response => {
                    this.Asset = response.data.data;
                    console.log(response);
                    this.sumItem = response.data.totalCount;
                    this.NumberPage(this.sumItem);
                    this.selectTick();
                    this.isLoading = false;
                    if (response.data.data.length == 0) {
                        this.isNoData = true;
                    }
                    else {
                        this.isNoData = false;
                    }
                    /*
                    *tính tổng các trường trong table
                    * Author : Bùi Quang Điệp
                    * Date : 09/08/2022
                    */
                    // set lại các biến tổng về 0
                    this.sumQuantity = 0;
                    this.sumPrice = 0;
                    this.sumDepreciation = 405;
                    this.sumAtrophy = 0;
                    // sử dụng vòng lặp để tính tổng 
                    for (const item of this.Asset) {
                        this.sumQuantity = (this.sumQuantity + item.quantity);
                        this.sumPrice = (this.sumPrice + item.cost);
                        this.sumDepreciation = 405;
                        this.sumAtrophy = (this.sumAtrophy + item.trackedYear);

                    }


                })
                .catch(error => {
                    this.isNoData = true;
                    alert(error);
                })


        },
        CheckNumber(item){
            debugger
            if(item == this.isCheckNumber)
            {
                return item;
            }
        },

        /**
        * Xử lý sự kiện Nhân bản 1 bản ghi
        * Author : Bùi Quang Điệp
        * Date:10/08/2022
        */
        ReplicationAsset(item) {

            this.emitter.emit("replication", item);

        },

        startDrag(evt, item) {
            evt.dataTransfer.dropEffect = 'move'
            evt.dataTransfer.effectAllowed = 'move'
            evt.dataTransfer.setData('itemID', item)
        },
        onDrop(evt, list) {
            const itemID = evt.dataTransfer.getData('itemID')
            const item = this.items.find((item) => item.id == itemID)
            item.list = list
        },

    },

    created() {
        /*
        * Gọi Hàm lấy dữ liệu đưa vào table
        * Author : Bùi Quang Điệp
        * Date : 14/08/2022
        */
        this.getAll();

        // Lấy thông tin bộ phận sử dụng từ API
        try {

            axios.get("http://localhost:13846/api/v1/Departments")
                .then(res => {
                    console.log(res);
                    this.dataDepartment = res.data;

                })
                .catch(res => {
                    console.log(res);
                })


        } catch (error) {
            console.log(error);
        }

        // lấy thông tin loại tài sản từ API
        try {

            axios.get("http://localhost:13846/api/v1/FixedAssetCategorys")
                .then(res => {
                    console.log(res);
                    this.dataCategory = res.data;


                })
                .catch(res => {
                    console.log(res);
                })


        } catch (error) {
            console.log(error);
        }
        this.NextPage(this.numberTwo);
       

    },
    mounted() {
        // Nhận lời gọi loading lại data từ dialog
        this.emitter.on("LoadData", (assetCode) => {
            // Gọi hàm load data
            this.titleWarning = [];
            this.dataTicks=[];
            this.assetCode = "";
            this.assetCode = assetCode;
            this.getAll();
            

        }),

            this.emitter.on("search", (searchArray) => {
                this.searchArray = searchArray;
                // Gọi hàm load data
                this.getAll();
            }),


            // Nhận số lượng bản ghi hiển thị
            this.emitter.on("dataPageSize", item => {
                this.recordNumber = item;
                this.NumberPage(this.sumItem);
                this.numberTwo=0;
                this.NextPage(this.numberTwo);

            })


    }


};
</script>

<style lang="scss" scoped>
</style>
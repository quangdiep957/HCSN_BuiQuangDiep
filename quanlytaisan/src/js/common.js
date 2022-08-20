export function formatCash(val){
    if(val){
        const str = val.toString()
        return str.replace(/^0+/, '').replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    }
    else{
        return 0;
    }
}
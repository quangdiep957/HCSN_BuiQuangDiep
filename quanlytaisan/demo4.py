
list = []
x=0
n = int(input("Nhap n= "))
while n<=8:
    n = int(input("N lon hon 8, nhap lai n: "))
while x <  n:
    x+=1
    value = int(input("Nhap phan tu: "))
    list.append(value)
tg = list[0]
for i in range(n):
    if tg > list[i]:
        tg = list[i]

if tg >= 0:
    print("Khong co so am")
    print("Câu d: ")
    print("Mảng không có phần tử âm nên không in mảng")
else:
    print("So am nho nhat list: ", tg) 
    #cau d
    print("Câu d: ")
    tg1 = 0
    d = []
    for i in range(n):
        if tg == list[i]:
            tg1 = i
    for i in range(tg1,n):
        d.append(list[i])    
    d.reverse()
    print(d)
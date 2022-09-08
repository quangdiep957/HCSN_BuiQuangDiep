List=['anh','en','banana']
res = []
# for x in List:
#     if 'a' in x:
#          res.insert(0,x)
# for x in List:
#     if 'a' not in x:
#         List.remove(x)
# for x in List:
#     if x.find('a'):
#         List.remove(x)
# for x in List:
#        if 'a' in x:
#           res.insert(0,x)
# List =[]
# List.extend(res)  

# def myFunc(x):
#         if 'a' in x:
#             return x
       
# filtered = filter(myFunc, List)
# print(list(filtered))
# for x in List:
#     if x.find('a'):
#         List.remove(x)
x=0
while x < len(List):
    if List[x].find('a'):
         List.remove(List[x])
    x=x+1
print(List)


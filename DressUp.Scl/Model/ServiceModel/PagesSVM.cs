using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class PagesSVM<T>
    {
        private IEnumerable<T> showList;

        public List<T> list { get; set; }
        public List<T> nowList { get; set; }
        public int pageNow { get; set; }
        public int pageTotal { get; set; }
        public int pageContent { get; set; }
        public PagesSVM(List<T> list)
        {
            this.list = list;
            pageNow = 1;
            pageContent = 5;
            setPageTotal(this.list);
            setNowList();
        }

        public PagesSVM()
        {
        }

        public PagesSVM(IEnumerable<T> showGoods)
        {
            this.showList = showGoods;
        }

        public List<T> getNowList()
        {
            return nowList;
        }
        public void setNowList()
        {
            if (this.list.Count == 0)
            {
                nowList = null;
            }
            else
            {
                if (pageNow == pageTotal)
                {
                    nowList = list.GetRange((pageNow - 1) * pageContent, list.Count - pageContent * (pageNow - 1));
                }
                else
                {
                    nowList = list.GetRange((pageNow - 1) * pageContent, pageContent);
                }
            }
        }
        public List<T> getAllList()
        {
            return list;
        }
        public void setAllList(List<T> list)
        {
            this.list = list;
            pageNow = 1;
            pageContent = 5;
            setPageTotal(this.list);
            setNowList();
        }
        public int getPageNow()
        {
            return pageNow;
        }
        public Boolean setPageNow(int pageNow)
        {
            if (pageNow > this.pageTotal || pageNow <= 0)
                return false;
            else
            {
                this.pageNow = pageNow;
                setNowList();
                return true;
            }
        }
        public int getPageContent()
        {
            return pageContent;
        }
        public void setPageContent(int pageContent)
        {
            this.pageContent = pageContent;
            setPageTotal(list);
            setNowList();
        }
        public int getPageTotal()
        {
            return pageTotal;
        }
        public void setPageTotal(List<T> list)
        {
            if (list.Count % this.getPageContent() == 0)
                pageTotal = list.Count / this.getPageContent();
            else
            {
                pageTotal = list.Count / this.getPageContent() + 1;
            }
        }
    }
}
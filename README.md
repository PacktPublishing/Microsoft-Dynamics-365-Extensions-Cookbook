


## $5 Tech Unlocked 2021!
[Buy and download this Book for only $5 on PacktPub.com](https://www.packtpub.com/product/microsoft-dynamics-365-extensions-cookbook/9781786464170)
-----
*If you have read this book, please leave a review on [Amazon.com](https://www.amazon.com/gp/product/1786464179).     Potential readers can then use your unbiased opinion to help them make purchase decisions. Thank you. The $5 campaign         runs from __December 15th 2020__ to __January 13th 2021.__*

# Microsoft Dynamics 365 Extensions Cookbook
This is the code repository for [Microsoft Dynamics 365 Extensions Cookbook](https://www.packtpub.com/application-development/microsoft-dynamics-365-extensions-cookbook?utm_source=github&utm_medium=repository&utm_content=9781786464170), published by [Packt](https://www.packtpub.com/?utm_source=github). It contains all the supporting project files necessary to work through the book from start to finish.

## About the Book
Microsoft Dynamics 365 is a powerful and versatile platform that has been around for more than a decade. With each release, the platform increased in richness and popularity. Being a moving target, it is often difficult to keep up with the features and capabilities introduced in the latest version. This book will help you narrow that knowledge gap in respect to the Dynamics CRM side of the product.
This Microsoft Dynamics 365 Extensions Cookbook not only covers classical configuration and customization extension topics, but also new Dynamics 365 features applicable to online Software-as-a-Service (SaaS) cloud ecosystems. Some topics are applicable to older versions
of Dynamics CRM, but most cover new patterns, frameworks, and tools that synergise well with the latest version. Unorthodox ideas, design patterns, and best practices are discussed throughout the book, differentiating it from other pieces of work.
With its cookbook format, this book sets out to enable you to harness the power of the Dynamics 365 platform, and caters to your unique circumstances through simple-to-follow step-by-step extension recipes.

## Instructions and Navigation
The code for Chapter 05 is organized into one folder, Chapter05 and the rest in one separate folder.

The code will look like the following:

```
packtNs.common.populateWithTodaysDate = function()
{
  if (Xrm.Page.getAttribute("packt_supervisor").getValue() !== null &&
     Xrm.Page.getAttribute("packt_postgraduatestartdate").getValue() === null)
  {
     Xrm.Page.getAttribute("packt_postgraduatestartdate").setValue(new Date());
  }
}
```
 
 ## Related Products
* [Microsoft Dynamics AX 2012 Reporting Cookbook](https://www.packtpub.com/application-development/microsoft-dynamics-ax-2012-reporting-cookbook?utm_source=github&utm_medium=repository&utm_content=9781849687720)

* [Programming Microsoft Dynamics™ NAV 2015](https://www.packtpub.com/big-data-and-business-intelligence/programming-microsoft-dynamics%E2%84%A2-nav-2015?utm_source=github&utm_medium=repository&utm_content=9781784394202)

* [Microsoft Dynamics NAV 2015 Professional Reporting](https://www.packtpub.com/big-data-and-business-intelligence/microsoft-dynamics-nav-2015-professional-reporting?utm_source=github&utm_medium=repository&utm_content=9781785284731)


### Download a free PDF

 <i>If you have already purchased a print or Kindle version of this book, you can get a DRM-free PDF version at no cost.<br>Simply click on the link to claim your free PDF.</i>
<p align="center"> <a href="https://packt.link/free-ebook/9781786464170">https://packt.link/free-ebook/9781786464170 </a> </p>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminMenu.ascx.cs" Inherits="Admin_controls_AdminMenu" %>
<div class="menucontainer l"> <!-- The wrapper for the admin dashboard -->
			<ul class="menu"> <!-- The first unordered list, all the main icons you see are within here -->
                <asp:PlaceHolder runat="server" ID="phCommerce" Visible ="true">
             <li><a href="#" class="block-13"><span class="fa fa-money"></span><span class="subtext"><%=Resources.CMS.Commerce %></span></a>
					<ul>
						<li>
							<div class="twobythree block-13">
								<ul>
									<li><a href="orders" class="block-13"><span class="fa fa-money"></span><span class="subtext"><%=Resources.CMS.Orders %></span></a></li>
                                    <li><a href="Orders?min=<%=CMS.CartStatus.New.ToString("d") %>&max=<%=CMS.CartStatus.Paid.ToString("d") %>" class="block-13"><span class="fa fa-money"></span><span class="subtext">New Orders</span></a></li>
                                    <li><a href="Orders?min=<%=CMS.CartStatus.ReadyToShip.ToString("d") %>&max=<%=CMS.CartStatus.ReadyToShip.ToString("d") %>" class="block-13"><span class="fa fa-money"></span><span class="subtext">Pending Shipping</span></a></li>
                                    <li><a href="Orders?min=<%=CMS.CartStatus.Shipped.ToString("d") %>&max=<%=CMS.CartStatus.Shipped.ToString("d") %>" class="block-13"><span class="fa fa-money"></span><span class="subtext">Shipped Orders</span></a></li>
                                    <li><a href="Orders?min=<%=CMS.CartStatus.Complete.ToString("d") %>&max=<%=CMS.CartStatus.Complete.ToString("d") %>" class="block-13"><span class="fa fa-money"></span><span class="subtext">Completed Orders</span></a></li>
                                    <li><a href="Orders?min=<%=CMS.CartStatus.Cancelled.ToString("d") %>&max=<%=CMS.CartStatus.Cancelled.ToString("d") %>" class="block-13"><span class="fa fa-money"></span><span class="subtext">Cancelled Orders</span></a></li>
                                    <li><a href="commerceusers" class="block-13"><span class="fa fa-group"></span><span class="subtext"><%=Resources.CMS.Users %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
			
             <asp:PlaceHolder runat="server" ID="phEvents" Visible="false">
             <li><a href="#" class="block-2"><i class="fa fa-calendar"></i><span class="subtext"><%=Resources.CMS.Events %></span></a>
             <ul>
						<li>
							<div class="twobythree block-2"> <!-- This is how the sub-menu will be laid out. Please see the documentation for how to use and example -->
								<ul>
									<li><a href="Default.aspx?mod=evt-g" class="block-2"><span class="fa fa-calendar"></span><span class="subtext"><%=Resources.CMS.Events %></span></a></li> <!-- Sub levels of the Link Manager element -->
									<li><a href="Default.aspx?mod=evt-p" class="block-2"><span class="fa fa-tasks"></span><span class="subtext"><%=Resources.CMS.EventAdd %></span></a></li>
									<li><a href="Default.aspx?mod=evtcat-g" class="block-2"><span class="fa fa-calendar-o"></span><span class="subtext"><%=Resources.CMS.EventCategories %></span></a></li>
									<li><a href="Default.aspx?mod=evtcat-p" class="block-2"><span class="fa fa-copy"></span><span class="subtext"><%=Resources.CMS.EventCategoryAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=evt-g&pub=0" class="block-2"><span class="fa fa-cloud"></span><span class="subtext"><%=Resources.CMS.EventsUnpublished %></span></a></li>
                                    <li><a href="Default.aspx?mod=evt-g&app=0" class="block-2"><span class="fa fa-exclamation"></span><span class="subtext"><%=Resources.CMS.EventsUnapproved %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
             </li>
             </asp:PlaceHolder>
             
             <asp:PlaceHolder runat="server" ID="phCatalog" Visible="true">
             <li><a href="#" class="block-4"><span class="fa fa-archive"></span><span class="subtext"><%=Resources.CMS.Catalog %></span></a>
					<ul>
						<li>
							<div class="fivebytwo block-4">
								<ul>
									<li><a href="Default.aspx?mod=pro-g" class="block-4"><span class="fa fa-tags"></span><span class="subtext"><%=Resources.CMS.Products %></span></a></li>
									<li><a href="Default.aspx?mod=pro-p" class="block-4"><span class="fa fa-tag"></span><span class="subtext"><%=Resources.CMS.ProductAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=procat-g" class="block-4"><span class="fa fa-briefcase"></span><span class="subtext"><%=Resources.CMS.Categories %></span></a></li>
									<li><a href="Default.aspx?mod=procat-p" class="block-4"><span class="fa fa-suitcase"></span><span class="subtext"><%=Resources.CMS.CategoryAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=probra-g" class="block-4"><span class="fa fa-pagelines"></span><span class="subtext"><%=Resources.CMS.Brands %></span></a></li>
									<li><a href="Default.aspx?mod=probra-p" class="block-4"><span class="fa fa-foursquare"></span><span class="subtext"><%=Resources.CMS.BrandAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=lok-g" class="block-4"><span class="fa fa-th"></span><span class="subtext"><%=Resources.CMS.Lookups %></span></a></li>
									<li><a href="Default.aspx?mod=lok-p" class="block-4"><span class="fa fa-th-list"></span><span class="subtext"><%=Resources.CMS.LookupAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=lokcat-g" class="block-4"><span class="fa fa-table"></span><span class="subtext"><%=Resources.CMS.LookupCategories %></span></a></li>
									<li><a href="Default.aspx?mod=lokcat-p" class="block-4"><span class="fa fa-th-large"></span><span class="subtext"><%=Resources.CMS.LookupCategoryAdd %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phReports" Visible="false">
             <li><a href="#" class="block-13"><span class="fa fa-bar-chart"></span><span class="subtext"><%=Resources.CMS.Reports %></span></a>
					<ul>
						<li>
							<div class="twobythree block-13">
								<ul>
									<li><a href="reportinventory" class="block-13"><span class="fa fa-bar-chart"></span><span class="subtext">Moving Average</span></a></li>
                                   </ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
                 <asp:PlaceHolder runat="server" ID="phArticles" Visible="true">
             <li><a href="#" class="block-1"><i class="fa fa-book"></i><span class="subtext"><%=Resources.CMS.Articles %></span></a>
             <ul>
						<li>
							<div class="twobythree block-1"> <!-- This is how the sub-menu will be laid out. Please see the documentation for how to use and example -->
								<ul>
									<li><a href="Default.aspx?mod=art-g" class="block-1"><span class="fa fa-book"></span><span class="subtext"><%=Resources.CMS.Articles %></span></a></li>
									<li><a href="Default.aspx?mod=art-p" class="block-1"><span class="fa fa-file"></span><span class="subtext"><%=Resources.CMS.ArticleAdd %></span></a></li>
									<li><a href="Default.aspx?mod=artcat-g" class="block-1"><span class="fa fa-clipboard"></span><span class="subtext"><%=Resources.CMS.ArticleCategories %></span></a></li>
									<li><a href="Default.aspx?mod=artcat-p" class="block-1"><span class="fa fa-copy"></span><span class="subtext"><%=Resources.CMS.ArticleCategoryAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=art-g&pub=0" class="block-1"><span class="fa fa-cloud"></span><span class="subtext"><%=Resources.CMS.ArticlesUnpublished %></span></a></li>
                                    <li><a href="Default.aspx?mod=art-g&app=0" class="block-1"><span class="fa fa-exclamation"></span><span class="subtext"><%=Resources.CMS.ArticlesUnapproved %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
             </li>
             </asp:PlaceHolder>
             <asp:PlaceHolder runat="server" ID="phGallery" Visible="true">
             <li><a href="#" class="block-5"><span class="fa fa-camera"></span><span class="subtext"><%=Resources.CMS.PhotoGallery %></span></a>
					<ul>
						<li>
							<div class="twobythree block-5">
								<ul>
									<li><a href="Default.aspx?mod=gal-g" class="block-5"><span class="fa fa-picture-o"></span><span class="subtext"><%=Resources.CMS.Photos %></span></a></li>
									<li><a href="Default.aspx?mod=gal-p" class="block-5"><span class="fa fa-ticket"></span><span class="subtext"><%=Resources.CMS.PhotoAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=galcat-g" class="block-5"><span class="fa fa-camera"></span><span class="subtext"><%=Resources.CMS.Albums %></span></a></li>
									<li><a href="Default.aspx?mod=galcat-p" class="block-5"><span class="fa fa-camera-retro"></span><span class="subtext"><%=Resources.CMS.AlbumAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=gal-g&pub=0" class="block-5"><span class="fa fa-flag"></span><span class="subtext"><%=Resources.CMS.PhotosUnpublished %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
             <asp:PlaceHolder runat="server" ID="phLibrary" Visible="false">
             <li><a href="#" class="block-10"><i class="fa fa-folder"></i><span class="subtext"><%=Resources.CMS.Library %></span></a>
             <ul>
						<li>
							<div class="fivebytwo block-10"> <!-- This is how the sub-menu will be laid out. Please see the documentation for how to use and example -->
								<ul>
									<li><a href="Default.aspx?mod=lib-g" class="block-10"><span class="fa fa-files-o"></span><span class="subtext"><%=Resources.CMS.Files%></span></a></li> <!-- Sub levels of the Link Manager element -->
									<li><a href="Default.aspx?mod=lib-p" class="block-10"><span class="fa fa-file"></span><span class="subtext"><%=Resources.CMS.FileAdd %></span></a></li>
									<li><a href="Default.aspx?mod=libcat-g" class="block-10"><span class="fa fa-folder"></span><span class="subtext"><%=Resources.CMS.LibraryCategories %></span></a></li>
									<li><a href="Default.aspx?mod=libcat-p" class="block-10"><span class="fa fa-folder-open"></span><span class="subtext"><%=Resources.CMS.LibraryCategoryAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=lib-g&pub=0" class="block-10"><span class="fa fa-file-o"></span><span class="subtext"><%=Resources.CMS.FilesUnpublished %></span></a></li>
                                    <li><a href="Default.aspx?mod=lib-g&app=0" class="block-10"><span class="fa fa-file-text"></span><span class="subtext"><%=Resources.CMS.FilesUnapproved %></span></a></li>
                                    <li><a href="Default.aspx?mod=lib-g&catid=-2" class="block-10"><span class="fa fa-music"></span><span class="subtext"><%=Resources.CMS.Audio %></span></a></li>
                                    <li><a href="Default.aspx?mod=lib-g&catid=-4" class="block-10"><span class="fa fa-video-camera"></span><span class="subtext"><%=Resources.CMS.Video %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
             </li>
             </asp:PlaceHolder>
             <asp:PlaceHolder runat="server" ID="phJobs" Visible="false">
				<li><a href="#" class="block-8"><span class="fa fa-suitcase"></span><span class="subtext"><%=Resources.CMS.Jobs %> </span></a> <!-- Another top level element -->
					<ul>
						<li>
							<div class="onebytwo block-8"> <!-- This is how the sub-menu will be laid out. Please see the documentation for how to use and example -->
								<ul>
                                <li><a href="Default.aspx?mod=job-g" class="block-8"><span class="fa fa-suitcase"></span><span class="subtext"><%=Resources.CMS.Jobs %></span></a></li>
									<li><a href="Default.aspx?mod=job-p" class="block-8"><span class="fa fa-money"></span><span class="subtext"><%=Resources.CMS.JobAdd %></span></a></li>
								</ul>
							</div>
						</li>
			 		</ul>
				</li>
                </asp:PlaceHolder>
             <asp:PlaceHolder runat="server" ID="phLinks" Visible="false">
             <li><a href="#" class="block-7"><i class="fa fa-link"></i><span class="subtext"><%=Resources.CMS.Links %></span></a>
             <ul>
						<li>
							<div class="twobytwo block-7"> <!-- This is how the sub-menu will be laid out. Please see the documentation for how to use and example -->
								<ul>
									<li><a href="Default.aspx?mod=lnk-g" class="block-7"><span class="fa fa-link"></span><span class="subtext"><%=Resources.CMS.Links %></span></a></li> <!-- Sub levels of the Link Manager element -->
									<li><a href="Default.aspx?mod=lnk-p" class="block-7"><span class="fa fa-unlink"></span><span class="subtext"><%=Resources.CMS.LinkAdd %></span></a></li>
									<li><a href="Default.aspx?mod=lnkcat-g" class="block-7"><span class="fa fa-external-link-square"></span><span class="subtext"><%=Resources.CMS.LinkCategories %></span></a></li>
									<li><a href="Default.aspx?mod=lnkcat-p" class="block-7"><span class="fa fa-external-link"></span><span class="subtext"><%=Resources.CMS.LinkCategoryAdd %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
             </li>
             </asp:PlaceHolder>
             
                <asp:PlaceHolder runat="server" ID="phCompanies" Visible="false">
             <li><a href="#" class="block-9"><i class="fa fa-plane"></i><span class="subtext"><%=Resources.CMS.Companies %></span></a>
             <ul>
						<li>
							<div class="twobytwo block-9"> <!-- This is how the sub-menu will be laid out. Please see the documentation for how to use and example -->
								<ul>
									<li><a href="Default.aspx?mod=cmp-g" class="block-9"><span class="fa fa-plane"></span><span class="subtext"><%=Resources.CMS.Companies%></span></a></li> <!-- Sub levels of the Link Manager element -->
									<li><a href="Default.aspx?mod=cmp-p" class="block-9"><span class="fa fa-flash"></span><span class="subtext"><%=Resources.CMS.CompanyAdd %></span></a></li>
									<li><a href="Default.aspx?mod=cmpcat-g" class="block-9"><span class="fa fa-key"></span><span class="subtext"><%=Resources.CMS.CompanyCategories %></span></a></li>
									<li><a href="Default.aspx?mod=cmpcat-p" class="block-9"><span class="fa fa-plus"></span><span class="subtext"><%=Resources.CMS.CompanyCategoryAdd %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
             </li>
             </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phOutlets" Visible="true">
             <li><a href="#" class="block-9"><i class="fa fa-plane"></i><span class="subtext"><%=Resources.CMS.Outlets %></span></a>
             <ul>
						<li>
							<div class="twobytwo block-9"> <!-- This is how the sub-menu will be laid out. Please see the documentation for how to use and example -->
								<ul>
									<li><a href="Default.aspx?mod=dir-g" class="block-9"><span class="fa fa-plane"></span><span class="subtext"><%=Resources.CMS.Outlets%></span></a></li> <!-- Sub levels of the Link Manager element -->
									<li><a href="Default.aspx?mod=dir-p" class="block-9"><span class="fa fa-flash"></span><span class="subtext"><%=Resources.CMS.OutletAdd %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
             </li>
             </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phPages">
             <li><a href="#" class="block-3"><span class="fa fa-paste"></span><span class="subtext"><%=Resources.CMS.Pages %></span></a>
					<ul>
						<li>
							<div class="onebytwo block-3">
								<ul>
									<li><a href="Default.aspx?mod=pag-g" class="block-3"><span class="fa fa-paste"></span><span class="subtext"><%=Resources.CMS.Pages %></span></a></li>
									<li><a href="Default.aspx?mod=pag-p" class="block-3"><span class="fa fa-edit"></span><span class="subtext"><%=Resources.CMS.PageAdd %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
              <asp:PlaceHolder runat="server" ID="phQuestions" Visible="false">
             <li><a href="#" class="block-11"><span class="fa fa-paste"></span><span class="subtext"><%=Resources.CMS.Questions %></span></a>
					<ul>
						<li>
							<div class="twobythree block-11">
								<ul>
									<li><a href="Default.aspx?mod=que-g" class="block-11"><span class="fa fa-paste"></span><span class="subtext"><%=Resources.CMS.Questions %></span></a></li>
									<li><a href="Default.aspx?mod=que-p" class="block-11"><span class="fa fa-edit"></span><span class="subtext"><%=Resources.CMS.QuestionAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=quecat-g" class="block-11"><span class="fa fa-edit"></span><span class="subtext"><%=Resources.CMS.QuestionCategories %></span></a></li>
                                    <li><a href="Default.aspx?mod=quecat-p" class="block-11"><span class="fa fa-edit"></span><span class="subtext"><%=Resources.CMS.QuestionCategoryAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=que-g&ans=1" class="block-11"><span class="fa fa-paste"></span><span class="subtext"><%=Resources.CMS.QuestionsAnswered %></span></a></li>
                                    <li><a href="Default.aspx?mod=que-g&ans=0" class="block-11"><span class="fa fa-paste"></span><span class="subtext"><%=Resources.CMS.QuestionsUnanswered %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
             <asp:PlaceHolder runat="server" ID="phPoll" Visible="false">
             <li><a href="#" class="block-12"><span class="fa fa-check-square"></span><span class="subtext"><%=Resources.CMS.Polls %></span></a>
					<ul>
						<li>
							<div class="onebytwo block-12">
								<ul>
									<li><a href="Default.aspx?mod=pol-g" class="block-12"><span class="fa fa-check-square"></span><span class="subtext"><%=Resources.CMS.Polls %></span></a></li>
									<li><a href="Default.aspx?mod=pol-p" class="block-12"><span class="fa fa-check"></span><span class="subtext"><%=Resources.CMS.PollAdd %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
                
             <asp:PlaceHolder runat="server" ID="phGuestbook" Visible="false">
             <li><a href="#" class="block-13"><span class="fa fa-comments"></span><span class="subtext"><%=Resources.CMS.Guestbook %></span></a>
					<ul>
						<li>
							<div class="onebytwo block-13">
								<ul>
									<li><a href="Default.aspx?mod=gue-g" class="block-13"><span class="fa fa-comments"></span><span class="subtext"><%=Resources.CMS.GuestbookEntries %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
             <asp:PlaceHolder runat="server" ID="phNewsletter" Visible="true">
             <li><a href="#" class="block-14"><span class="fa fa-envelope"></span><span class="subtext"><%=Resources.CMS.Newsletter %></span></a>
					<ul>
						<li>
							<div class="onebytwo block-14">
								<ul>
									<li><a href="Default.aspx?mod=newsub-g" class="block-14"><span class="fa fa-envelope"></span><span class="subtext"><%=Resources.CMS.NewsletterSubscribers %></span></a></li>
								</ul>
							</div>
						</li>
					</ul>
				</li>
             </asp:PlaceHolder>
             <asp:PlaceHolder runat="server" ID="phSettings">
				<li><a href="#" class="block-6"><span class="fa fa-cogs"></span><span class="subtext"><%=Resources.CMS.Settings %> </span></a> <!-- Another top level element -->
					<ul>
						<li>
							<div class="twobythree block-6"> <!-- This is how the sub-menu will be laid out. Please see the documentation for how to use and example -->
								<ul>
                                <li><a href="Default.aspx?mod=usr-g" class="block-6"><span class="fa fa-group"></span><span class="subtext"><%=Resources.CMS.Users %></span></a></li>
									<li><a href="Default.aspx?mod=usr-p" class="block-6"><span class="fa fa-user"></span><span class="subtext"><%=Resources.CMS.UserAdd %></span></a></li>
									<li><a href="Default.aspx?mod=men-g" class="block-6"><span class="fa fa-sitemap"></span><span class="subtext"><%=Resources.CMS.Menu %></span></a></li>
									<li><a href="Default.aspx?mod=men-p" class="block-6"><span class="fa fa-random"></span><span class="subtext"><%=Resources.CMS.MenuAdd %></span></a></li>
									<li><a href="Default.aspx?mod=cnt-g" class="block-6"><span class="fa fa-globe"></span><span class="subtext"><%=Resources.CMS.ContentFiles %></span></a></li>
									<li><a href="Default.aspx?mod=cnt-p" class="block-6"><span class="fa fa-upload"></span><span class="subtext"><%=Resources.CMS.ContentAdd %></span></a></li>
                                    <li><a href="Default.aspx?mod=face-p" class="block-6"><span class="fa fa-facebook"></span><span class="subtext"><%=Resources.CMS.FacebookLogin %></span></a></li>
								</ul>
							</div>
						</li>
			 		</ul>
				</li>
                </asp:PlaceHolder>
			</ul>
		</div>

